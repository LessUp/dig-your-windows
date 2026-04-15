# DigYourWindows

[![CI](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)
[![Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

English | [简体中文](README.zh-CN.md)

> 📖 **Documentation**: [https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

**Windows Deep Diagnostics Tool** — One-click hardware info collection, event log analysis, reliability records, system health scoring, and optimization recommendations.

## Features

| Feature | Description |
|---------|-------------|
| 🔍 **Hardware Info** | CPU, GPU, RAM, Disk (incl. SMART), Network adapters, USB devices |
| 📊 **Real-Time Monitoring** | CPU temp/load/frequency, GPU temp/load/VRAM, network traffic |
| 📋 **Event Log Analysis** | Auto-extract System/Application errors and warnings |
| 📈 **Reliability Records** | Windows Reliability Monitor data with trend visualization |
| ✅ **Health Score** | Comprehensive stability, performance, memory, disk assessment |
| 💡 **Optimization Advice** | Auto-generated targeted recommendations |
| 📄 **Report Export** | HTML and JSON formats |
| 🎨 **Theme Support** | Dark/Light theme toggle |

## Tech Stack

| Component | Technology | Purpose |
|-----------|------------|---------|
| Runtime | .NET 10.0 + WPF | Desktop application framework |
| UI Library | WPF-UI 4.0 | Fluent Design components |
| MVVM | CommunityToolkit.Mvvm 8.4 | Data binding & commands |
| Charts | ScottPlot 5.1 | Performance visualization |
| Hardware | LibreHardwareMonitor 0.9 | CPU/GPU temperature, load, frequency |
| Testing | xUnit 2.9 + FsCheck 2.16 | Unit & property-based tests |

## Quick Start

### Prerequisites

- Windows 10/11
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Build & Run

```powershell
# Restore dependencies
dotnet restore DigYourWindows.slnx

# Build
dotnet build DigYourWindows.slnx -c Release

# Run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ Some features (GPU monitoring, SMART data) require administrator privileges.

### Testing

```powershell
# Run all tests
dotnet test DigYourWindows.slnx

# Run with coverage
dotnet test DigYourWindows.slnx --collect:"XPlat Code Coverage"
```

### Publish

```powershell
# Generate Framework-dependent and Self-contained builds
.\scripts\publish.ps1

# Build installer (requires Inno Setup 6)
.\scripts\build-installer.ps1
```

## Project Structure

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # Core business logic
│   │   ├── Models/              # Data models (by domain)
│   │   ├── Services/            # Service layer
│   │   └── Exceptions/          # Custom exceptions
│   └── DigYourWindows.UI/       # WPF user interface
│       ├── ViewModels/          # MVVM view models
│       └── Converters/          # Value converters
├── tests/
│   └── DigYourWindows.Tests/    # Unit & property tests
│       ├── Unit/                # Unit tests
│       └── Property/            # Property-based tests
├── docs/                        # VitePress documentation
├── installer/                   # Inno Setup scripts
├── scripts/                     # Build & release scripts
├── changelog/                   # Change logs
├── Directory.Build.props        # Shared MSBuild properties
└── DigYourWindows.slnx          # Solution file
```

## Architecture Highlights

- **Shared Build Properties** — `Directory.Build.props` centralizes TargetFramework, Nullable, version
- **Singleton Hardware Monitor** — `HardwareMonitorProvider` shares LibreHardwareMonitor instance
- **Efficient Event Log** — `EventLogReader` with structured XML queries for server-side filtering
- **CancellationToken Support** — All I/O-bound operations are cancellable
- **Buffered Logging** — `StreamWriter` with rotation for efficient log writing
- **Interface-based DI** — All services have interfaces for testability

## Release Process

Push a `v*` tag to trigger automatic release:

```powershell
git tag v1.0.0
git push origin v1.0.0
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

[MIT License](LICENSE) - Copyright © 2025-2026 LessUp
