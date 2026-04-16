# Contributing Guide

Thank you for your interest in contributing to DigYourWindows!

## Quick Setup

```powershell
# 1. Fork the repository on GitHub

# 2. Clone your fork
git clone https://github.com/YOUR_USERNAME/dig-your-windows.git
cd dig-your-windows

# 3. Add upstream remote
git remote add upstream https://github.com/LessUp/dig-your-windows.git

# 4. Build and test
dotnet restore
dotnet build
dotnet test
```

## Development Workflow

```powershell
# Sync with upstream
git checkout master
git fetch upstream
git merge upstream/master

# Create feature branch
git checkout -b feature/my-feature

# Make changes and commit
git add .
git commit -m "feat(service): add new capability"

# Push and create PR
git push origin feature/my-feature
```

## Code Standards

### C# Naming

| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `DiagnosticService` |
| Methods | PascalCase | `GetHardwareInfo()` |
| Properties | PascalCase | `ComputerName` |
| Fields (private) | _camelCase | `_logService` |
| Parameters | camelCase | `cancellationToken` |

### Commit Convention (Conventional Commits)

```
<type>(<scope>): <description>

[optional body]
```

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `refactor`: Code refactoring
- `test`: Tests
- `chore`: Build/tooling

**Example**:
```powershell
git commit -m "feat(monitor): add GPU memory monitoring

- Implement real-time VRAM tracking
- Add to performance dashboard"
```

## Pull Request Checklist

- [ ] Code follows project style guidelines
- [ ] Tests added/updated
- [ ] All tests pass (`dotnet test`)
- [ ] Documentation updated
- [ ] Commits follow conventional format

## Getting Help

- [Documentation](https://lessup.github.io/dig-your-windows/)
- [GitHub Discussions](https://github.com/LessUp/dig-your-windows/discussions)
- [GitHub Issues](https://github.com/LessUp/dig-your-windows/issues)

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).
