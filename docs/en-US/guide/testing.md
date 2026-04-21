# Testing Guide

This document introduces the testing strategy and best practices for DigYourWindows.

## Testing Frameworks

| Dependency | Version | Purpose |
|------------|---------|---------|
| xUnit | 2.9.2 | Test framework |
| FsCheck | 2.16.6 | Property-based testing framework |
| FsCheck.Xunit | 2.16.6 | xUnit integration |
| Microsoft.NET.Test.Sdk | 17.11.1 | Test runner |
| coverlet.collector | 6.0.2 | Code coverage |

## Test Structure

```
DigYourWindows.Tests/
├── Unit/                           # Unit tests
│   ├── ReportServiceTests.cs       # Report service tests
│   ├── DiagnosticCollectorServiceTests.cs
│   └── PerformanceServiceTests.cs  # Performance analysis tests
├── Property/                       # Property tests
│   └── ReportServicePropertyTests.cs
├── Integration/                    # Integration tests (reserved)
├── FsCheckConfig.cs               # FsCheck configuration
├── Usings.cs                      # Global usings
└── DigYourWindows.Tests.csproj
```

## Running Tests

### Basic Commands

```powershell
# Run all tests
dotnet test DigYourWindows.slnx

# Release mode
dotnet test DigYourWindows.slnx -c Release

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Filtering Tests

```powershell
# Filter by class name
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# Filter by method name
dotnet test --filter "FullyQualifiedName~SerializeToJson_ThenDeserialize"

# Filter by category
dotnet test --filter "Category=Unit"
```

### Code Coverage

```powershell
# Collect coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate report (requires reportgenerator tool)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

## Unit Tests

### Test Naming Convention

```
[MethodName]_[Scenario]_[ExpectedResult]
```

Examples:
- `SerializeToJson_ThenDeserialize_ShouldPreserveSelectedFields`
- `CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException`

### Test Example

```csharp
public class ReportServiceTests
{
    [Fact]
    public void SerializeToJson_ThenDeserialize_ShouldPreserveSelectedFields()
    {
        // Arrange
        var service = new ReportService();
        var data = new DiagnosticData
        {
            Hardware = new HardwareData { ComputerName = "TEST-PC" },
            CollectedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var json = service.SerializeToJson(data, indented: false);
        var deserialized = service.DeserializeFromJson(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal("TEST-PC", deserialized!.Hardware.ComputerName);
    }
}
```

### Using Stub/Mock

```csharp
private sealed class StubHardwareService : IHardwareService
{
    private readonly Func<CancellationToken, HardwareData> _handler;

    public StubHardwareService(Func<CancellationToken, HardwareData> handler)
    {
        _handler = handler;
    }

    public HardwareData GetHardwareInfo(CancellationToken cancellationToken = default)
        => _handler(cancellationToken);
}

private sealed class SpyLogService : ILogService
{
    public List<(string Message, Exception? Exception)> Errors { get; } = new();

    public void Info(string message) { }
    public void Warn(string message) { }
    public void LogError(string message, Exception? exception = null)
        => Errors.Add((message, exception));
}
```

## Property-Based Testing

### Configuration

`FsCheckConfig.cs` defines the default configuration:

```csharp
public static class FsCheckConfig
{
    public static Configuration Default => new Configuration
    {
        MaxNbOfTest = 100,    // Minimum 100 iterations
        QuietOnSuccess = true
    };
}

// Custom attribute
public class PropertyTestAttribute : PropertyAttribute
{
    public PropertyTestAttribute()
    {
        MaxTest = 100;
        QuietOnSuccess = true;
    }
}
```

### Test Example

```csharp
public class ReportServicePropertyTests
{
    [PropertyTest]
    public void SerializeDeserialize_RoundTripPreservesHardwareCoreFields(
        NonEmptyString computerName,
        NonEmptyString osVersion,
        NonEmptyString cpuBrand,
        NonNegativeInt cpuCoresRaw,
        NonNegativeInt memoryGBRaw)
    {
        var cpuCores = (uint)(cpuCoresRaw.Get % 128);
        var memoryGB = (ulong)(memoryGBRaw.Get % 128);
        var totalMemory = memoryGB * 1024UL * 1024UL * 1024UL;

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = osVersion.Get,
                CpuBrand = cpuBrand.Get,
                CpuCores = cpuCores,
                TotalMemory = totalMemory
            },
            CollectedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var service = new ReportService();
        var json = service.SerializeToJson(data, indented: false);
        var deserialized = service.DeserializeFromJson(json);

        Assert.NotNull(deserialized);
        Assert.Equal(data.Hardware.ComputerName, deserialized!.Hardware.ComputerName);
        Assert.Equal(data.Hardware.TotalMemory, deserialized.Hardware.TotalMemory);
    }
}
```

## Test Coverage Requirements

| Requirement ID | Description | Test Type |
|---------------|-------------|-----------|
| 2.1 | Schema validation and data parsing | Unit test |
| 2.2 | HTML report generation validation | Unit test |
| 2.3 | Performance score range validation | Unit test |
| 2.4 | Malformed input handling | Unit test |
| 2.5 | JSON serialization round-trip | Property test |

## Best Practices

### 1. Test Isolation

Each test should run independently, not depending on other tests' state:

```csharp
// Good: Each test creates a new instance
[Fact]
public void Test1()
{
    var service = new ReportService();  // New instance
    // ...
}
```

### 2. Test Boundary Conditions

```csharp
[Theory]
[InlineData(0, "Unknown")]
[InlineData(-1, "Unknown")]
[InlineData(90, "Excellent")]
[InlineData(89, "Good")]
[InlineData(50, "Fair")]
public void GetHealthGrade_ShouldReturnCorrectGrade(int score, string expectedGrade)
{
    // Test boundary conditions
}
```

### 3. Test Exception Paths

```csharp
[Fact]
public async Task CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException()
{
    using var cts = new CancellationTokenSource();
    var service = CreateService();

    await Assert.ThrowsAsync<OperationCanceledException>(
        () => service.CollectAsync(3, cancellationToken: cts.Token));
}
```

### 4. Avoid Over-Mocking

For simple data transformations, prefer using real objects:

```csharp
// Good: Use real ReportService directly
var service = new ReportService();

// Avoid: Over-mocking simple logic
var mockService = new Mock<IReportService>();
mockService.Setup(s => s.SerializeToJson(It.IsAny<DiagnosticData>()))
           .Returns("{}");
```

## CI Integration

Tests run automatically in the CI pipeline:

```yaml
# .github/workflows/ci.yml
- name: Test
  run: dotnet test DigYourWindows.slnx -c Release --no-restore
```
