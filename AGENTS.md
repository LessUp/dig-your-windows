# DigYourWindows Agent Guide

Spec-Driven Development (SDD) with OpenSpec for a .NET 10 WPF Windows diagnostics application.

## OpenSpec Workflow

This project uses OpenSpec for specification management. Use the following slash commands:

### Checking Specs

- `/opsx:explore` - Think through ideas before committing
- `openspec list` - List all spec domains
- `openspec show <domain>` - View specific spec (e.g., `openspec show hardware`)
- `openspec validate` - Validate all specs

### Proposing Changes

1. **Start a proposal**: `/opsx:propose <change-name>`
   - Creates `openspec/changes/<change-name>/` with proposal.md
2. **Design the change**: `/opsx:continue` or `/opsx:ff`
   - Creates design.md with implementation details
   - Creates delta specs in `changes/<change-name>/specs/`
3. **Generate tasks**: Creates tasks.md with actionable items
4. **Implement**: `/opsx:apply` - Work through tasks

### Delta Spec Format

Delta specs describe changes relative to current specs:

```markdown
## ADDED Requirements

### Requirement: New Feature
[New requirement description]

## MODIFIED Requirements

### Requirement: Existing Feature
~~Old behavior~~
New behavior

## REMOVED Requirements

### Requirement: Deprecated Feature
[Reason for removal]
```

### Completing Changes

- `/opsx:archive` - Merge delta specs and move to archive/

## Mandatory Workflow: Spec-First

**Before writing any code**, check OpenSpec for existing requirements:

1. **Check specs**: `openspec list` then `openspec show <domain>`
2. **For new features**: `/opsx:propose <feature-name>`
3. **Wait for approval**: User must approve proposal before implementation
4. **Write tests**: Against spec acceptance criteria
5. **No gold-plating**: Do not add features beyond spec definition

If user instructions conflict with existing specs, **stop and ask** whether to create a change proposal first.

## OpenSpec Directory Structure

```
openspec/
├── config.yaml           # Configuration
├── specs/               # Source of truth
│   ├── architecture/    # 核心架构决策
│   ├── data/           # 数据模型和校验
│   ├── export/         # 报告导出 API
│   ├── hardware/       # 硬件监控规范
│   ├── testing/        # 测试策略
│   └── features/       # 产品功能
├── changes/            # Active change proposals
└── archive/            # Completed changes
```

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
| `openspec/` | **OpenSpec source of truth** | Markdown |
| `docs/` | VitePress user documentation (zh-CN primary, en-US mirror) | Markdown |
| `scripts/` | PowerShell build/publish scripts | `.ps1` |
| `installer/` | Inno Setup installer config | `.iss` |

## Technical Constraints

- **Target**: `net10.0-windows10.0.19041.0` (Windows 10 Build 19041+ only)
- **UI**: WPF with WPF-UI 4.0 (Fluent Design), ScottPlot 5.1 for charts
- **MVVM**: CommunityToolkit.Mvvm 8.4 (source generators, no manual `INotifyPropertyChanged`)
- **Hardware**: LibreHardwareMonitorLib 0.9.4 (requires admin for GPU/SMART)
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

types: feat, fix, docs, refactor, test, chore, specs
scopes: core, ui, openspec, docs, tests, build
```

**OpenSpec-specific commits**:
- `specs(architecture): add GPU monitoring architecture` - New spec content
- `change(export): propose PDF export feature` - New change proposal
- `archive(hardware): complete SMART attributes update` - Archived change

## Documentation

- **Specs** (`/openspec/`): Technical requirements and design decisions
- **User docs** (`/docs/`): VitePress site, bilingual (zh-CN primary)
- Do not duplicate spec content in user docs—link instead

## Release

Push `v*` tag to trigger automated release with FDD and SCD artifacts:

```powershell
git tag v1.1.0
git push origin v1.1.0
```
