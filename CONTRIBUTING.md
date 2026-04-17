# Contributing to DigYourWindows

Thank you for your interest in contributing to DigYourWindows! This document provides guidelines and instructions for contributing.

## 🚀 Quick Setup

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

## 📝 Development Workflow

This project follows **Spec-Driven Development (SDD)**. Before implementing any feature or fix, please:

1. **Review existing specs** in `/specs/` directory
2. **Create or update specs** if they don't exist or need changes
3. **Implement code** based on specs
4. **Write tests** that validate against spec acceptance criteria

### Spec-First Workflow

For new features or significant changes:

1. **Create RFC**: Write a technical design document in `/specs/rfc/`
2. **Update product specs**: Define features and acceptance criteria in `/specs/product/`
3. **Update API/data specs**: Modify interface definitions in `/specs/api/` and `/specs/db/`
4. **Get review**: Discuss the spec with maintainers before implementation
5. **Implement**: Write code that 100% complies with specs
6. **Test**: Write tests based on acceptance criteria in specs

See [AGENTS.md](AGENTS.md) for detailed AI agent workflow instructions.

### Branch Workflow

```powershell
# Sync with upstream
git checkout main
git fetch upstream
git merge upstream/main

# Create feature branch
git checkout -b feature/my-feature

# Make changes and commit
git add .
git commit -m "feat(scope): add new capability

- Implement feature according to spec RFC-XXXX
- Add tests covering acceptance criteria"

# Push and create PR
git push origin feature/my-feature
```

## 📋 Code Standards

### C# Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `DiagnosticService` |
| Methods | PascalCase | `GetHardwareInfo()` |
| Properties | PascalCase | `ComputerName` |
| Fields (private) | _camelCase | `_logService` |
| Parameters | camelCase | `cancellationToken` |
| Interfaces | IPascalCase | `IHardwareService` |

### Code Style

- Use implicit usings (configured in `Directory.Build.props`)
- Enable nullable reference types
- Follow recommended analysis level
- Treat warnings as errors
- Use XML documentation for public APIs

### Commit Convention (Conventional Commits)

```
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, semicolons)
- `refactor`: Code refactoring
- `test`: Test changes
- `chore`: Build/tooling changes

**Scopes** (examples):
- `core`: Core business logic
- `ui`: User interface
- `specs`: Specification documents
- `docs`: Documentation
- `tests`: Test code
- `build`: Build configuration

**Examples**:
```powershell
# Feature with scope
git commit -m "feat(core): add GPU memory monitoring

- Implement real-time VRAM tracking in GpuMonitorService
- Add to performance dashboard
- Update spec/product/hardware-detection.md"

# Bug fix
git commit -m "fix(eventlog): handle empty event log

- Check for empty log before querying
- Add unit test for edge case"

# Documentation
git commit -m "docs(specs): add RFC for caching strategy"
```

## 📁 Specification Documents

### Creating New Specs

When creating a new spec document:

1. **Choose the right directory**:
   - `/specs/product/` - Product features and acceptance criteria
   - `/specs/rfc/` - Technical design decisions
   - `/specs/api/` - API interface definitions
   - `/specs/db/` - Data model specifications
   - `/specs/testing/` - Test strategy and BDD specs

2. **Use the standard header**:
   ```markdown
   # Spec Title: Feature Name

   **Status**: Draft | Accepted | Implemented
   **Date**: YYYY-MM-DD
   **Version**: X.Y.Z
   **Priority**: Low | Medium | High | Critical
   ```

3. **Include acceptance criteria** with checkboxes:
   ```markdown
   **Acceptance Criteria**:
   - [ ] Criterion 1
   - [ ] Criterion 2
   ```

4. **Reference related specs**:
   ```markdown
   ## Related Documents
   - [RFC-0001: Core Architecture](../rfc/0001-core-architecture.md)
   ```

### Updating Existing Specs

- Mark status as "Implemented" when code is complete
- Update version number for significant changes
- Add changelog entry if spec changed substantially
- Ensure code remains compliant after changes

## ✅ Pull Request Checklist

Before submitting a PR, ensure:

- [ ] Code follows project style guidelines
- [ ] Specs created/updated for new features or changes
- [ ] Tests added/updated to cover acceptance criteria
- [ ] All tests pass (`dotnet test`)
- [ ] No new warnings (`dotnet build`)
- [ ] Documentation updated (both English and Chinese if applicable)
- [ ] Commits follow conventional format
- [ ] PR description references relevant specs

## 🧪 Testing Requirements

### Test Coverage

- **Unit tests**: 80%+ line coverage minimum
- **Property tests**: 100 iterations per property
- **Integration tests**: Conditional execution on Windows

### Running Tests

```powershell
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test category
dotnet test --filter "Category=Unit"

# Run specific test class
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

### Writing Tests

- Follow naming: `MethodName_Scenario_ExpectedBehavior`
- Include XML documentation comments
- Use factory methods for test data creation
- Test both success and failure paths

## 📚 Documentation

### User Documentation

Located in `/docs/`:
- `/docs/setup/` - Environment setup guides
- `/docs/tutorials/` - User tutorials
- `/docs/architecture/` - Architecture overview (links to specs)
- `/docs/assets/` - Static resources

### VitePress Site

The documentation site uses VitePress. To run locally:

```powershell
cd docs
npm install
npm run dev
```

### Updating Documentation

- Keep user-facing docs (guides, tutorials) in `/docs/`
- Reference specs from user docs using GitHub blob links
- Update both English and Chinese versions
- Don't duplicate spec content in user docs - link to specs instead

## 🐛 Reporting Issues

When reporting issues:

1. **Check existing issues** to avoid duplicates
2. **Provide context**:
   - Windows version
   - .NET version
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots if applicable
3. **Reference relevant specs** if the issue relates to a spec
4. **Label appropriately** (bug, enhancement, documentation, etc.)

## 💬 Getting Help

- [Documentation Site](https://lessup.github.io/dig-your-windows/)
- [GitHub Discussions](https://github.com/LessUp/dig-your-windows/discussions)
- [GitHub Issues](https://github.com/LessUp/dig-your-windows/issues)

## 📄 License

By contributing, you agree that your contributions will be licensed under the [MIT License](https://github.com/LessUp/dig-your-windows/blob/main/LICENSE).
