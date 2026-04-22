# DigYourWindows Agent Guide

Spec-Driven Development (SDD) workflow for a .NET 10 WPF Windows diagnostics application.

## Mandatory Workflow: Spec-First

**Before writing any code**, check `/specs/` for existing requirements. If implementing a new feature:

1. **Review specs** in `/specs/product/`, `/specs/rfc/`, `/specs/api/`, `/specs/db/`
2. **Create/update specs** before coding (RFC for architecture, product/ for features)
3. **Wait for user confirmation** on spec changes before implementing
4. **Write tests** against spec acceptance criteria
5. **No gold-plating**: Do not add features beyond spec definition

If user instructions conflict with existing specs, **stop and ask** whether to update the spec first.

## Build & Test Commands

```powershell
# Use .slnx (new XML solution format), NOT .sln
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release --no-restore
dotnet test DigYourWindows.slnx -c Release --no-restore

# Filtered testing
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
dotnet test --filter "Category=Unit"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Project Structure

| Directory | Purpose | Output |
|-----------|---------|--------|
| `src/DigYourWindows.Core/` | Business logic, services, models | Class library |
| `src/DigYourWindows.UI/` | WPF application (MVVM, XAML) | `WinExe` |
| `tests/DigYourWindows.Tests/` | xUnit + FsCheck property tests | Test project |
| `specs/` | **SDD source of truth** - PRDs, RFCs, API specs | Markdown |
| `docs/` | VitePress user documentation (zh-CN primary, en-US mirror) | Markdown |
| `scripts/` | PowerShell build/publish scripts | `.ps1` |
| `installer/` | Inno Setup installer config | `.iss` |

## Technical Constraints

- **Target**: `net10.0-windows10.0.19041.0` (Windows 10 Build 19041+ only)
- **UI**: WPF with WPF-UI 4.0 (Fluent Design), ScottPlot 5.1 for charts
- **MVVM**: CommunityToolkit.Mvvm 8.4 (source generators, no manual `INotifyPropertyChanged`)
- **Hardware**: LibreHardwareMonitorLib 0.9.4 (requires admin for GPU/ SMART)
- **Testing**: xUnit + FsCheck for property-based testing

## Code Conventions

From `Directory.Build.props`: implicit usings enabled, nullable reference types enforced, warnings treated as errors.

| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `DiagnosticService` |
| Methods | PascalCase | `GetHardwareInfo()` |
| Properties | PascalCase | `ComputerName` |
| Private fields | _camelCase | `_logService` |
| Interfaces | IPascalCase | `IHardwareService` |

## Commit Convention

```
<type>(<scope>): <description>

types: feat, fix, docs, refactor, test, chore
scopes: core, ui, specs, docs, tests, build
```

## Documentation

- **Specs** (`/specs/`): Technical requirements and design decisions
- **User docs** (`/docs/`): VitePress site, bilingual (zh-CN primary)
- Do not duplicate spec content in user docs—link instead

## Release

Push `v*` tag to trigger automated release with FDD and SCD artifacts:

```powershell
git tag v1.1.0
git push origin v1.1.0
```
