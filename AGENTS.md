# DigYourWindows — AI Agent Reference

**Project**: Windows深度诊断工具 | .NET 10 WPF | Spec-Driven Development (OpenSpec)
**Repo**: `LessUp/dig-your-windows` | **Docs**: `https://lessup.github.io/dig-your-windows/`
**Status**: v1.2.0 Archive-Ready — 稳定、功能完整、维护模式

---

## Architecture: Two-Layer

```
DigYourWindows.UI          →  WPF (MVVM, CommunityToolkit.Mvvm 8.4, ScottPlot 5.1, WPF-UI 4.0)
DigYourWindows.Core        →  Business logic (services, models, exceptions) — class library
DigYourWindows.Tests       →  xUnit 2.9 + FsCheck 2.16 (Unit/ PropertyTests/ Integration/)
```

## Core Services (DigYourWindows.Core/Services/)

| Service | Responsibility | Key Dependency |
|---------|---------------|----------------|
| `HardwareService` | CPU/GPU/RAM/Disk/Network/USB via WMI | `System.Management` |
| `CpuMonitorService` | Real-time CPU temp/load/freq | `HardwareMonitorProvider` |
| `GpuMonitorService` | Real-time GPU temp/load/VRAM | `HardwareMonitorProvider` |
| `NetworkMonitorService` | Adapter info + bandwidth tracking | WMI + PerformanceCounter |
| `DiskSmartService` | SMART health data (NVMe + SATA) | `LibreHardwareMonitorLib` |
| `EventLogService` | System/Application errors via XML query | `EventLogReader` |
| `ReliabilityService` | Windows Reliability Monitor data | WMI `Win32_ReliabilityRecords` |
| `PerformanceService` | Health scoring: stability 40%+perf 30%+mem 15%+disk 15% | Pure computation |
| `ReportService` | HTML + JSON export | `System.Text.Json` |
| `DiagnosticCollectorService` | Orchestrates all collection with progress+cancellation | All above |
| `HardwareMonitorProvider` | Thread-safe singleton wrapping `LibreHardwareMonitor.Computer` | `LibreHardwareMonitorLib 0.9.4` |
| `ConfigurationService` | JSON app settings persistence | `System.Text.Json` |
| `FileLogService` / `LogService` | Buffered file logging | `StreamWriter` |

## Data Models (DigYourWindows.Core/Models/)

| File | Key Types |
|------|-----------|
| `DiagnosticData.cs` | `DiagnosticData` (root), `SystemInfo` |
| `HardwareData.cs` | `HardwareData`, `MemoryData`, `NetworkAdapterInfo` |
| `DiskModels.cs` | `DiskInfoData`, `DiskSmartData`, `SmartAttribute` |
| `ComputeModels.cs` | `CpuData`, `GpuInfoData` |
| `EventModels.cs` | `LogEvent`, `ReliabilityRecord` |
| `PerformanceAnalysisData.cs` | `PerformanceAnalysisData`, `Recommendation` |
| `CollectionModels.cs` | `DiagnosticCollectionResult`, `DiagnosticCollectionProgress` |

## Custom Exceptions (DigYourWindows.Core/Exceptions/)

| Exception | Factory Methods | Key Properties |
|-----------|----------------|----------------|
| `ServiceException` | `ServiceException.OperationFailed(...)` | `ErrorType`, `ServiceName` |
| `ReportException` | `ReportException.InvalidData(...)`, `.FileAccessError(...)` | `ErrorType`, `MissingField`, `Path` |
| `WmiException` | `WmiException.QueryFailed(...)` | `ErrorType`, `Resource`, `Query` |

## Build Commands

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release --no-restore
dotnet test DigYourWindows.slnx -c Release --no-restore
dotnet test --filter "Category=Unit"
dotnet test --collect:"XPlat Code Coverage"
```

## OpenSpec Workflow (MANDATORY before coding)

```bash
# 1. Check relevant spec
cat openspec/specs/<domain>/spec.md
# domains: architecture | data | export | hardware | features | testing | workflow

# 2. For changes to existing features
# Edit delta spec in openspec/changes/<name>/specs/<domain>/spec.md

# 3. For bugs — no proposal needed, just fix and update spec status if relevant
```

**Rule**: No code without a matching spec. No features beyond spec. No gold-plating.

## Code Conventions

```csharp
// ✅ Correct
public sealed class HardwareMonitorProvider : IHardwareMonitorProvider, IDisposable
{
    private readonly object _lock = new();
    private ILogService _logService;  // _camelCase fields
    public Computer Computer { get; }  // PascalCase props/methods
}

// ❌ Wrong: manual INotifyPropertyChanged, Newtonsoft.Json, non-nullable without justification
```

- **MVVM**: `[ObservableProperty]` + `[RelayCommand]` source generators — never manual implementation
- **Async**: All I/O ops take `CancellationToken cancellationToken = default`
- **Nullable**: All `?` must be handled — no `!` suppressions without comment
- **Warnings = Errors**: Zero tolerance; `TreatWarningsAsErrors` is on

## Commit Convention

```
<type>(<scope>): <description>
types: feat fix docs refactor test chore specs
scopes: core ui tests docs openspec build
```

## Maintenance Mode

- ✅ Bug fixes only (no new features)
- ✅ Security updates as needed
- ✅ Documentation corrections
- ❌ No new features or architecture changes

## Final Status

**v1.2.0** is the stable release. Project is in maintenance mode.
- All planned features implemented
- All known bugs fixed
- Test coverage meets 80% threshold
- Documentation complete and synchronized

