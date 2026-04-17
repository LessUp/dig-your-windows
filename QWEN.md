# DigYourWindows — Project Context

## Project Overview

**DigYourWindows** is a Windows deep diagnostics desktop application built with .NET 10.0 and WPF. It provides one-click hardware information collection, event log analysis, reliability records, system health scoring (0-100), and optimization recommendations. The project targets Windows 10/11 (Build 19041+) and uses the Fluent Design UI style via WPF-UI.

**Repository**: `https://github.com/LessUp/dig-your-windows`
**Documentation**: `https://lessup.github.io/dig-your-windows/`
**License**: MIT

## Architecture

The project follows a clean two-layer architecture with Spec-Driven Development (SDD):

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # Core business logic (services, models, exceptions)
│   └── DigYourWindows.UI/       # WPF user interface (MVVM, converters, XAML)
├── tests/
│   └── DigYourWindows.Tests/    # Unit (64), Property (0), Integration (3) tests
├── specs/                       # Specification documents (SDD) — Single Source of Truth
│   ├── product/                 # Product requirements (PRD)
│   ├── rfc/                     # Technical design documents (RFCs)
│   ├── api/                     # API specifications
│   ├── db/                      # Data model specifications
│   └── testing/                 # Test strategy & BDD specs
├── docs/                        # VitePress documentation site (bilingual)
│   ├── zh-CN/                   # Chinese documentation (primary)
│   ├── en-US/                   # English documentation
│   ├── public/                  # Static assets (images, icons, manifest)
│   └── .vitepress/              # VitePress config, theme, PWA
├── installer/                   # Inno Setup installation scripts
├── scripts/                     # Build & release PowerShell scripts
└── .github/workflows/           # CI, Pages deploy, Release automation
```

### Key Services (DigYourWindows.Core)

| Service | Responsibility |
|---------|---------------|
| `HardwareService` | Collect CPU, GPU, RAM, disk, network, USB info via WMI |
| `CpuMonitorService` | Real-time CPU temp/load/frequency monitoring |
| `GpuMonitorService` | Real-time GPU temp/load/VRAM monitoring |
| `NetworkMonitorService` | Network adapter info and traffic tracking |
| `DiskSmartService` | Disk SMART health data reading |
| `EventLogService` | System/Application error & warning log extraction |
| `ReliabilityService` | Windows Reliability Monitor data |
| `PerformanceService` | Health scoring algorithm (stability 40%, performance 30%, memory 15%, disk 15%) |
| `ReportService` | HTML and JSON report export |
| `DiagnosticCollectorService` | Orchestrates all collection steps with progress reporting |
| `HardwareMonitorProvider` | Singleton wrapping LibreHardwareMonitor `Computer` instance |
| `ConfigurationService` | JSON-based app settings persistence |
| `FileLogService` | File-based logging with buffering |

### UI Architecture

- **MVVM Pattern**: Uses `CommunityToolkit.Mvvm` (source generators for `[ObservableProperty]`, `[RelayCommand]`)
- **Main View**: `MainWindow.xaml` — single-window tabbed interface
- **Main ViewModel**: `MainViewModel.cs` — orchestrates all services, manages state machine for diagnostic collection
- **Charts**: ScottPlot 5.1 for real-time performance visualization
- **Converters**: `CountToVisibilityConverter`, `StringToBrushConverter`, `NullConverters`

### Key Design Decisions

1. **Singleton `HardwareMonitorProvider`**: LibreHardwareMonitor's `Computer` is a heavyweight resource; shared via thread-safe singleton with double-checked locking
2. **EventLogReader + XML queries**: Server-side filtering for efficient event log reading (95% less data transfer)
3. **All async operations support `CancellationToken`**: Ensures UI responsiveness and prevents resource leaks
4. **Model separation by domain**: Data models split across 8 focused files (`HardwareData.cs`, `DiskModels.cs`, `ComputeModels.cs`, etc.)
5. **Dependency Injection**: All services registered via `IServiceCollection` with appropriate lifetimes

## Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| Runtime | .NET + WPF | 10.0 |
| UI Library | WPF-UI | 4.0.3 |
| MVVM | CommunityToolkit.Mvvm | 8.4.0 |
| Charts | ScottPlot.WPF | 5.1.57 |
| Hardware Monitor | LibreHardwareMonitorLib | 0.9.4 |
| DI | Microsoft.Extensions.DependencyInjection | 10.0.0 |
| Testing | xUnit | 2.9.2 |
| Property Testing | FsCheck.Xunit | 2.16.6 |
| Docs | VitePress + PWA | 1.6.4 |

## Building and Running

### Prerequisites
- Windows 10/11 (Build 19041+)
- .NET 10.0 SDK
- Node.js 22+ (for docs build only)
- Administrator privileges (for full feature access — GPU monitoring, SMART data)

### Build & Run Commands

```powershell
# Restore dependencies
dotnet restore DigYourWindows.slnx

# Build
dotnet build DigYourWindows.slnx -c Release

# Run the application
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj

# Run all tests
dotnet test DigYourWindows.slnx

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific tests
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# Build documentation
npm run docs:build

# Serve documentation locally
npm run docs:dev
```

### Release Build

Push a `v*` tag to trigger automated release:
```powershell
git tag v1.1.0
git push origin v1.1.0
```
This triggers `.github/workflows/release.yml` which builds FDD (framework-dependent) and SCD (self-contained) releases and publishes to GitHub Releases.

## Development Conventions

### Spec-Driven Development (SDD)

**This project strictly follows SDD.** All code implementation must be based on `/specs/` documents as the single source of truth:

1. **Review specs first** — before writing any code, read relevant specs in `/specs/product/`, `/specs/rfc/`, `/specs/api/`, `/specs/db/`
2. **Spec-first** — for new features or interface changes, create/update specs before coding
3. **100% spec compliance** — code must match spec definitions exactly (naming, types, paths, status codes, error formats)
4. **No gold-plating** — do not add features not defined in specs
5. **Test against specs** — tests must validate acceptance criteria from specs

### C# Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| Class | PascalCase | `DiagnosticService` |
| Method | PascalCase | `GetHardwareInfo()` |
| Property | PascalCase | `ComputerName` |
| Field (private) | `_camelCase` | `_logService` |
| Parameter | camelCase | `cancellationToken` |
| Interface | `I`PascalCase | `IHardwareService` |

### Commit Messages (Conventional Commits)

```
<type>(<scope>): <description>

[optional body]
```

Types: `feat`, `fix`, `docs`, `refactor`, `test`, `chore`

### Testing

- **Unit tests**: Located in `tests/DigYourWindows.Tests/Unit/` — test individual classes and methods
- **Property tests**: Located in `tests/DigYourWindows.Tests/Property/` — use FsCheck for property-based testing
- **Integration tests**: Located in `tests/DigYourWindows.Tests/Integration/` — test service interactions
- **Implicit usings**: `Xunit`, `FsCheck`, `FsCheck.Xunit` are globally imported via `Usings.cs`
- Tests reference both `DigYourWindows.Core` and `DigYourWindows.UI` projects

### CI/CD Workflows

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `ci.yml` | Push/PR to master | Build + Test on Windows |
| `pages.yml` | Push to master (docs changes) | Build & deploy VitePress docs to GitHub Pages |
| `release.yml` | Push `v*` tag | Build FDD/SCD releases, publish to GitHub Releases |

## Important Notes

- **Windows-only project**: The application only runs on Windows (WPF). Development/build on Linux/macOS is limited to code editing and docs
- **Admin privileges required** for full feature access (GPU monitoring, SMART data). The app gracefully degrades without admin rights
- **Version**: Currently `1.0.0` in `Directory.Build.props`; `1.1.0` is the latest git tag
- **Documentation**: Bilingual (zh-CN primary, en-US mirror). VitePress config uses `withPwa` from `@vite-pwa/vitepress` for PWA support
- **No `changelog/` directory**: The project previously had a `/changelog/` folder that was consolidated into root `CHANGELOG.md` and locale-specific `docs/*/changelog.md`
