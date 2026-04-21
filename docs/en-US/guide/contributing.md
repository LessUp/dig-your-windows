# Contributing Guide

Thank you for your interest in contributing to DigYourWindows! This document will help you participate in project development.

## Code of Conduct

- Respect all contributors
- Accept constructive criticism
- Focus on what is best for the community

## How to Contribute

### Reporting Bugs

1. Search [Issues](https://github.com/LessUp/dig-your-windows/issues) to confirm the bug hasn't been reported
2. Create a new Issue using the Bug report template
3. Include the following information:
   - Operating system version
   - .NET version
   - Steps to reproduce
   - Expected behavior vs actual behavior

### Submitting Feature Requests

1. Create a new Issue using the Feature request template
2. Clearly describe the feature requirement and use case
3. Explain how the feature would benefit the project

### Submitting Code

#### Development Environment Setup

```powershell
# Clone the repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Restore dependencies
dotnet restore DigYourWindows.slnx

# Verify build
dotnet build DigYourWindows.slnx

# Run tests
dotnet test DigYourWindows.slnx
```

#### Code Standards

- Follow [Microsoft C# Coding Conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use EditorConfig configuration (project includes `.editorconfig`)
- All public APIs must have XML documentation comments
- Unit tests use `[MethodName]_[Scenario]_[ExpectedResult]` naming format

#### Commit Convention

Use [Conventional Commits](https://www.conventionalcommits.org/) format:

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code formatting (no functionality change)
- `refactor`: Code refactoring
- `test`: Test-related changes
- `chore`: Build/tooling changes

**Examples**:
```
feat(monitor): add GPU memory usage monitoring

fix(reliability): handle empty records in timeline builder

docs(guide): add troubleshooting section
```

#### Pull Request Process

1. Fork the repository and create a feature branch
   ```powershell
   git checkout -b feat/my-feature
   ```

2. Make changes and write tests

3. Ensure all tests pass
   ```powershell
   dotnet test DigYourWindows.slnx
   ```

4. Commit your changes
   ```powershell
   git commit -m "feat: add amazing feature"
   ```

5. Push to your fork and create a Pull Request

6. Wait for CI to pass and code review

### Pull Request Checklist

- [ ] Code follows project coding standards
- [ ] Necessary tests have been added
- [ ] All tests pass
- [ ] Documentation updated (if applicable)
- [ ] Commit messages follow Conventional Commits specification

## Project Structure

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/   # Core business logic
│   └── DigYourWindows.UI/     # WPF user interface
├── tests/
│   └── DigYourWindows.Tests/  # Test project
├── docs/                      # VitePress documentation site
└── scripts/                   # Build scripts
```

## Testing Guide

For detailed testing guide, refer to [Testing Guide](./testing.md).

### Running Tests

```powershell
# Run all tests
dotnet test

# Run specific tests
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# Collect coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Release Process

1. Update version number in `Directory.Build.props`
2. Update `CHANGELOG.md`
3. Create and push Git tag
   ```powershell
   git tag v1.x.x
   git push origin v1.x.x
   ```
4. GitHub Actions automatically builds and publishes

## Getting Help

- Check the [Documentation](https://lessup.github.io/dig-your-windows/)
- Ask in [Discussions](https://github.com/LessUp/dig-your-windows/discussions)
- Create an [Issue](https://github.com/LessUp/dig-your-windows/issues)

## License

This project is licensed under the MIT License. By submitting code, you agree that your contributions will be licensed under the same license.
