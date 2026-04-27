# DigYourWindows — Copilot Instructions

Windows deep diagnostics desktop app. .NET 10 + WPF. **Finalization phase**: fix bugs, improve tests, no new features.

## Project Structure

```
src/DigYourWindows.Core/    # Business logic — services, models, exceptions
src/DigYourWindows.UI/      # WPF app — MVVM, XAML, converters
tests/DigYourWindows.Tests/ # xUnit + FsCheck (Unit/ PropertyTests/ Integration/)
openspec/specs/             # Source of truth — read before touching code
```

## Key Rules

1. **Check spec first**: Before any change, read `openspec/specs/<domain>/spec.md`
2. **No new features**: Only bug fixes and test improvements for v1.2.0
3. **Zero warnings**: `TreatWarningsAsErrors=true` — all builds must be clean
4. **No Newtonsoft.Json**: Use `System.Text.Json` exclusively
5. **MVVM via source generators**: `[ObservableProperty]` / `[RelayCommand]` only — no manual INotifyPropertyChanged
6. **Nullable**: Handle all nullables properly; no `!` without comment

## Tech Stack

| Layer | Package | Version |
|-------|---------|---------|
| UI | WPF-UI | 4.0.3 |
| MVVM | CommunityToolkit.Mvvm | 8.4.0 |
| Charts | ScottPlot.WPF | 5.1.57 |
| Hardware | LibreHardwareMonitorLib | 0.9.4 |
| DI | Microsoft.Extensions.DependencyInjection | 10.0.0 |
| Testing | xUnit + FsCheck.Xunit | 2.9.2 / 2.16.6 |

## C# Conventions

```csharp
// PascalCase: types, methods, properties
// _camelCase: private fields
// IPascalCase: interfaces
// camelCase: parameters, locals

public sealed class HardwareMonitorProvider : IHardwareMonitorProvider, IDisposable
{
    private readonly ILogService _logService;      // _camelCase
    public Computer Computer { get; }              // PascalCase
    public void Dispose() { /* cleanup */ }
}
```

## Services Architecture

`HardwareMonitorProvider` (singleton) → `CpuMonitorService` + `GpuMonitorService`
`DiagnosticCollectorService` orchestrates: Hardware → CPU → GPU → Network → Disk → Events → Reliability → Performance scoring
`ReportService` exports: JSON (System.Text.Json) + HTML (string builder with embedded CSS)

## Build & Test

```powershell
dotnet build DigYourWindows.slnx -c Release
dotnet test DigYourWindows.slnx -c Release
dotnet test --filter "Category=Unit"
```

## Commit Format

`<type>(<scope>): <description>` — types: feat fix docs refactor test chore specs

## Finalization Rules (v1.2.0)

- ✅ Fix bugs in existing features
- ✅ Improve test coverage (property tests)
- ✅ Update docs to match implementation
- ❌ No new features (CLI mode, benchmark, etc. cancelled)
- ❌ No dependency upgrades unless security issue
- ❌ No architecture changes

## Known Limitations

| Issue | Status |
|-------|--------|
| `GetCpuBrandScore` Intel/AMD generation bonus unreachable | Won't fix — breaks score consistency |
| Some services lack direct unit tests | Property tests provide coverage |
