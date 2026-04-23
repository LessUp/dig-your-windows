# Contributing to DigYourWindows

## Development Setup

```powershell
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release
dotnet test DigYourWindows.slnx -c Release
```

## Spec-Driven Development (SDD)

This project uses **OpenSpec** as the single source of truth. All code changes must be backed by a spec.

### Before Writing Code

```bash
# 1. Check what specs exist
find openspec/specs -name "*.md"

# 2. View a specific domain
cat openspec/specs/hardware/spec.md

# 3. Propose a change (for new features or significant modifications)
# Creates openspec/changes/<name>/ with proposal.md
```

### Workflow

```
Check Specs → Propose Change → Design → Tasks → Implement → /review → Commit
```

1. **Check**: Review `openspec/specs/<domain>/spec.md` for the area you're changing
2. **Propose**: Create `openspec/changes/<change-name>/proposal.md` describing the change
3. **Design**: Write `design.md` with implementation approach and delta specs
4. **Tasks**: Break into `tasks.md` — atomic, testable items
5. **Implement**: Write code that 100% matches the spec; no gold-plating
6. **Test**: Tests must validate spec acceptance criteria
7. **Archive**: Move completed changes to `openspec/archive/`

### OpenSpec Directory

```
openspec/
├── config.yaml          # Domains and rules
├── specs/               # Source of truth by domain
│   ├── architecture/    # System design decisions
│   ├── data/            # Data models and validation
│   ├── export/          # Report export API
│   ├── hardware/        # Hardware monitoring contracts
│   ├── features/        # Product features and user stories
│   ├── testing/         # Test strategy and coverage requirements
│   └── workflow/        # Development process and tooling
├── changes/             # Active proposals (in-flight work)
└── archive/             # Completed and merged changes
```

## Code Standards

### C# Conventions

| Type | Convention | Example |
|------|------------|---------|
| Class | PascalCase | `DiagnosticService` |
| Method | PascalCase | `GetHardwareInfo()` |
| Property | PascalCase | `ComputerName` |
| Private field | `_camelCase` | `_logService` |
| Interface | `I`PascalCase | `IHardwareService` |

### Build Rules (from `Directory.Build.props`)
- Nullable reference types: **enabled** — no `!` suppressions without justification
- Warnings as errors — zero warnings allowed
- Implicit usings — no redundant `using System;`

### Commit Format (Conventional Commits)

```
<type>(<scope>): <description>

types: feat, fix, docs, refactor, test, chore, specs
scopes: core, ui, tests, docs, openspec, build
```

OpenSpec-specific:
```
specs(hardware): add GPU memory threshold scenarios
change(export): propose PDF export feature
archive(hardware): complete SMART attributes update
```

## Branch Strategy

This is a **solo** project in finalization phase:
- `master` is the only long-lived branch
- Short-lived feature branches (`<1 day`) for complex changes
- Merge immediately after CI passes — never let branches accumulate
- Tag releases: `git tag v1.2.0 && git push origin v1.2.0`

## Testing

```powershell
dotnet test DigYourWindows.slnx -c Release --no-restore
dotnet test --filter "Category=Unit"
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
dotnet test --collect:"XPlat Code Coverage"
```

- **Unit tests** (`tests/.../Unit/`): 85%+ coverage for services
- **Property tests** (`tests/.../PropertyTests/`): FsCheck property-based validation
- **Integration tests** (`tests/.../Integration/`): Windows-only, skipped on unsupported environments

Test naming: `MethodName_Scenario_ExpectedBehavior`

## Pull Request Checklist

- [ ] Spec exists for the change (`openspec/specs/`)
- [ ] `dotnet build` passes with zero warnings
- [ ] `dotnet test` passes
- [ ] Both English and Chinese docs updated (if user-visible change)
- [ ] Conventional commit format used

