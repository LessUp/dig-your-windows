# DigYourWindows

[![Deploy Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/docs.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/docs.yml)

[简体中文](README.md) | English

> 📖 **Documentation**: [https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

Windows deep diagnostics tool — one-click hardware info collection, event log analysis, reliability records, system health scoring and optimization recommendations.

## Features

- **Hardware Info** — CPU, GPU, RAM, Disk (incl. SMART), Network adapters, USB devices
- **Real-Time Monitoring** — CPU temp/load/freq, GPU temp/load/VRAM, network traffic
- **Event Log Analysis** — Auto-extract System/Application errors and warnings
- **Reliability Records** — Read Windows Reliability Monitor data with trend visualization
- **Health Score** — Comprehensive stability, performance, memory, disk health assessment
- **Optimization Advice** — Auto-generated targeted recommendations
- **Report Export** — HTML and JSON formats
- **Dark/Light Theme** — One-click toggle

## Tech Stack

- **.NET 10.0** + **WPF**
- **WPF-UI** (Fluent Design)
- **CommunityToolkit.Mvvm**
- **ScottPlot** (Charts)
- **LibreHardwareMonitor** (Hardware monitoring)
- **xUnit** + **FsCheck** (Testing)

## Project Structure

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # Core business logic
│   │   ├── Models/              # Data models (split by type)
│   │   ├── Services/            # Service layer
│   │   └── Exceptions/          # Custom exceptions
│   └── DigYourWindows.UI/       # WPF user interface
├── tests/
│   └── DigYourWindows.Tests/    # Unit & property tests
├── docs/                        # Docs & data schema
├── installer/                   # Inno Setup scripts
├── scripts/                     # Build & release scripts
├── Directory.Build.props        # Shared MSBuild properties
└── DigYourWindows.slnx          # Solution file
```

## Quick Start

### Requirements

- Windows 10/11
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Build

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release
```

### Run

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> Some features (GPU monitoring, SMART data) require administrator privileges.

### Test

```powershell
dotnet test DigYourWindows.slnx
```

### Publish

```powershell
.\scripts\publish.ps1
.\scripts\build-installer.ps1   # Requires Inno Setup 6
```

## Architecture Highlights

- **Shared `Directory.Build.props`** — Centralized TargetFramework, Nullable, version management
- **Shared hardware monitor instance** — `HardwareMonitorProvider` singleton for CPU/GPU
- **Efficient event log reading** — `EventLogReader` with structured XML queries
- **CancellationToken support** — All long-running operations are cancellable
- **Buffered logging** — `StreamWriter` instead of per-write `File.AppendAllText`

## License

[MIT License](LICENSE)
