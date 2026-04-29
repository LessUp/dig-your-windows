# DigYourWindows — Claude Code Instructions

**Project**: Windows Deep Diagnostics Tool | .NET 10 WPF | OpenSpec-Driven Development
**Status**: v1.2.0 Archive-Ready — Stable, feature-complete, maintenance mode

---

## Architecture Overview

```
DigYourWindows.UI          →  WPF (MVVM, CommunityToolkit.Mvvm 8.4, ScottPlot 5.1, WPF-UI 4.0)
DigYourWindows.Core        →  Business logic (services, models, exceptions) — class library
DigYourWindows.Tests       →  xUnit 2.9 + FsCheck 2.16 (Unit/PropertyTests/Integration/)
```

## OpenSpec Workflow (MANDATORY)

1. **Before coding**: Read `openspec/specs/<domain>/spec.md`
2. **Domains**: architecture | data | export | hardware | features | testing | workflow
3. **Changes**: Create proposal in `openspec/changes/` → implement → archive
4. **Rule**: No code without matching spec. No features beyond spec.

## Code Conventions

### MVVM
- Use `[ObservableProperty]` and `[RelayCommand]` source generators
- Never manual `INotifyPropertyChanged` implementation

### JSON
- Use `System.Text.Json` exclusively
- Never use `Newtonsoft.Json`

### Nullable
- Handle all `?` properly
- No `!` suppressions without explanatory comment

### Naming
- PascalCase: types, methods, properties, public fields
- _camelCase: private fields
- IPascalCase: interfaces
- camelCase: parameters, locals

## Key Services

| Service | Role |
|---------|------|
| `HardwareMonitorProvider` | Singleton wrapper for LibreHardwareMonitor.Computer |
| `DiagnosticCollectorService` | Orchestrates all data collection |
| `PerformanceService` | Health scoring algorithm |
| `ReportService` | HTML/JSON export |

## Build & Test

```powershell
dotnet build DigYourWindows.slnx -c Release  # Zero warnings (TreatWarningsAsErrors=true)
dotnet test DigYourWindows.slnx -c Release   # All tests must pass
dotnet test --collect:"XPlat Code Coverage"  # Coverage >= 80%
```

## Maintenance Mode

- ✅ Bug fixes only (no new features)
- ✅ Security updates as needed
- ✅ Documentation corrections
- ❌ No new features or architecture changes

## Known Limitations

| Issue | Status |
|-------|--------|
| `GetCpuBrandScore` Intel/AMD generation bonus unreachable | Won't fix — changing scores breaks consistency |
| Some services lack direct unit tests | Property tests provide indirect coverage |

## Commit Format

```
<type>(<scope>): <description>
types: feat fix docs refactor test chore specs
scopes: core ui tests docs openspec build
```

---

For personal preferences, see `CLAUDE.local.md`.
