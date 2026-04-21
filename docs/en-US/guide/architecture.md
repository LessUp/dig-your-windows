# Architecture

This document details the technical architecture and design decisions of DigYourWindows.

## Tech Stack

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| Runtime | .NET + WPF | 10.0 | Desktop application framework |
| UI Library | WPF-UI | 4.0 | Fluent Design components |
| MVVM | CommunityToolkit.Mvvm | 8.4 | Data binding & commands |
| Charts | ScottPlot | 5.1 | Performance visualization |
| Hardware Monitor | LibreHardwareMonitor | 0.9 | CPU/GPU temperature, load, frequency |
| WMI | System.Management | 10.0 | Windows management information queries |
| Testing | xUnit + FsCheck | 2.9 / 2.16 | Unit tests + Property tests |

## Project Structure

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # Core business logic
│   │   ├── Models/              # Data models (split by domain)
│   │   │   ├── DiagnosticData.cs
│   │   │   ├── HardwareData.cs
│   │   │   ├── DiskModels.cs
│   │   │   ├── ComputeModels.cs
│   │   │   ├── EventModels.cs
│   │   │   ├── DeviceModels.cs
│   │   │   ├── CollectionModels.cs
│   │   │   └── PerformanceAnalysisData.cs
│   │   ├── Services/            # Service layer
│   │   │   ├── HardwareService.cs
│   │   │   ├── CpuMonitorService.cs
│   │   │   ├── GpuMonitorService.cs
│   │   │   ├── NetworkMonitorService.cs
│   │   │   ├── DiskSmartService.cs
│   │   │   ├── EventLogService.cs
│   │   │   ├── ReliabilityService.cs
│   │   │   ├── PerformanceService.cs
│   │   │   ├── ReportService.cs
│   │   │   ├── DiagnosticCollectorService.cs
│   │   │   ├── LogService.cs
│   │   │   ├── HardwareMonitorProvider.cs
│   │   │   └── ScoringConfiguration.cs
│   │   └── Exceptions/          # Custom exceptions
│   │       ├── ServiceException.cs
│   │       ├── ReportException.cs
│   │       └── WmiException.cs
│   └── DigYourWindows.UI/       # WPF user interface
│       ├── ViewModels/          # MVVM view models
│       │   └── MainViewModel.cs
│       ├── Converters/          # Value converters
│       │   ├── CountToVisibilityConverter.cs
│       │   ├── NullConverters.cs
│       │   └── StringToBrushConverter.cs
│       ├── App.xaml.cs          # Application entry + DI composition root
│       └── MainWindow.xaml.cs   # Main window
├── tests/
│   └── DigYourWindows.Tests/    # Test project
│       ├── Unit/                # Unit tests
│       │   ├── ReportServiceTests.cs
│       │   ├── DiagnosticCollectorServiceTests.cs
│       │   └── PerformanceServiceTests.cs
│       ├── Property/            # Property tests
│       │   └── ReportServicePropertyTests.cs
│       ├── FsCheckConfig.cs     # FsCheck configuration
│       └── Usings.cs            # Global usings
├── docs/                        # VitePress documentation site
├── installer/                   # Inno Setup installer scripts
├── scripts/                     # Build and release scripts
├── Directory.Build.props        # Shared MSBuild properties
└── DigYourWindows.slnx          # Solution file
```

## Core Architecture Design

### 1. Shared Build Properties (`Directory.Build.props`)

Centralized management of common MSBuild properties:

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0-windows10.0.19041.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
  </PropertyGroup>
  <!-- Version, Authors, Product, etc. -->
</Project>
```

**Benefits**:
- Avoid duplicate configuration in each `.csproj`
- Unified target framework and language version
- Simplified version management

### 2. Singleton Hardware Monitoring (`HardwareMonitorProvider`)

LibreHardwareMonitor's `Computer` object is a heavyweight resource, shared via singleton pattern:

```csharp
public sealed class HardwareMonitorProvider : IHardwareMonitorProvider
{
    private Computer? _computer;

    public Computer Computer { get; }

    public HardwareMonitorProvider()
    {
        _computer = new Computer { IsCpuEnabled = true, IsGpuEnabled = true };
        _computer.Open();
    }
}
```

**Benefits**:
- Avoid creating multiple `Computer` instances
- CPU/GPU monitoring services share the same instance
- Unified lifecycle management

### 3. Efficient Event Log Reading (`EventLogService`)

Using `EventLogReader` + structured XML queries for server-side filtering:

```csharp
var queryXml = $@"
  <QueryList>
    <Query Id='0' Path='{logName}'>
      <Select Path='{logName}'>
        *[System[(Level=2 or Level=3) and 
          TimeCreated[@SystemTime&gt;='{cutoffStr}']]]
      </Select>
    </Query>
  </QueryList>";

using var reader = new EventLogReader(new EventLogQuery(logName, PathType.LogName, queryXml));
```

**Benefits**:
- Server-side filtering reduces data transfer
- Replaces iterating through all entries, greatly improving performance
- Supports UTC time range queries

### 4. Full CancellationToken Support

All time-consuming operations support cancellation:

```csharp
public interface IHardwareService
{
    HardwareData GetHardwareInfo(CancellationToken cancellationToken = default);
}

public interface IDiagnosticCollectorService
{
    Task<DiagnosticCollectionResult> CollectAsync(
        int daysBack,
        IProgress<DiagnosticCollectionProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
```

**Benefits**:
- Ensures UI responsiveness
- Supports user cancellation of long-running operations
- Prevents resource leaks

### 5. Model Split Design

Data models are split by responsibility into separate files:

| File | Content |
|------|---------|
| `DiagnosticData.cs` | Diagnostic data overview |
| `HardwareData.cs` | Hardware information |
| `DiskModels.cs` | Disk and SMART data |
| `ComputeModels.cs` | CPU/GPU real-time data |
| `EventModels.cs` | Event logs and reliability records |
| `DeviceModels.cs` | Network/USB device information |
| `PerformanceAnalysisData.cs` | Performance analysis results |
| `CollectionModels.cs` | Collection progress and results |

**Benefits**:
- Single Responsibility Principle
- Easy to maintain and extend
- Clear data boundaries

### 6. Buffered Log Service (`FileLogService`)

Using `StreamWriter` instead of per-call `File.AppendAllText`:

```csharp
private StreamWriter _writer;

private void Write(string level, string message, Exception? exception)
{
    lock (_lock)
    {
        CheckLogRotation();  // Rotate by date and size
        _writer.WriteLine($"{timestamp} [{level}] {message}");
    }
}
```

**Benefits**:
- Reduced I/O overhead
- Log rotation support (by date + size)
- Automatic old log cleanup (keeps 7 days)

## Dependency Injection Configuration

All services are configured at application startup:

```csharp
private static void ConfigureServices(IServiceCollection services)
{
    // UI
    services.AddSingleton<MainWindow>();
    services.AddSingleton<MainViewModel>();

    // Core Services
    services.AddSingleton<ILogService, FileLogService>();
    services.AddSingleton<IReportService, ReportService>();
    services.AddSingleton<IDiagnosticCollectorService, DiagnosticCollectorService>();

    // Hardware Monitoring
    services.AddSingleton<IHardwareMonitorProvider, HardwareMonitorProvider>();
    services.AddSingleton<ICpuMonitorService, CpuMonitorService>();
    services.AddSingleton<IGpuMonitorService, GpuMonitorService>();
    services.AddSingleton<INetworkMonitorService, NetworkMonitorService>();
    services.AddSingleton<IDiskSmartService, DiskSmartService>();
    services.AddSingleton<IHardwareService, HardwareService>();

    // Analysis
    services.AddSingleton<IReliabilityService, ReliabilityService>();
    services.AddSingleton<IEventLogService, EventLogService>();
    services.AddSingleton<ISystemInfoProvider, WmiSystemInfoProvider>();
    services.AddSingleton<IPerformanceService, PerformanceService>();
}
```

## Exception Handling Strategy

Custom exception types provide rich contextual information:

| Exception Type | Purpose | Special Properties |
|---------------|---------|-------------------|
| `ServiceException` | Service layer errors | `ErrorType`, `ServiceName`, `FailedServices` |
| `ReportException` | Report generation errors | `ErrorType`, `Path`, `MissingField` |
| `WmiException` | WMI query errors | `ErrorType`, `Resource`, `Query` |

All exceptions provide factory methods for easy creation:

```csharp
throw ServiceException.CollectionFailed("HardwareService", "WMI timeout");
throw ReportException.InvalidData("JSON content is empty");
throw WmiException.AccessDenied("Win32_Processor");
```
