# Testing Guide

This document covers DigYourWindows' testing strategy, framework choices, and best practices.

## Testing Strategy Overview

| Test Type | Framework | Focus | Location |
|-----------|-----------|-------|----------|
| Unit Tests | xUnit | Service logic, edge cases | `tests/Unit/` |
| Property Tests | FsCheck | Data invariants, random inputs | `tests/Property/` |
| Integration Tests | xUnit | End-to-end workflows | `tests/Integration/` |

## Test Framework

| Dependency | Version | Purpose |
|------------|---------|---------|
| xUnit | 2.9.2 | Unit testing framework |
| FsCheck | 2.16.6 | Property-based testing |
| FsCheck.Xunit | 2.16.6 | xUnit integration |
| coverlet.collector | 6.0.2 | Code coverage |

## Running Tests

::: code-group

```powershell [Run All Tests]
dotnet test DigYourWindows.slnx
```

```powershell [Release Mode]
dotnet test DigYourWindows.slnx -c Release
```

```powershell [Filter by Class]
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

```powershell [With Coverage]
dotnet test --collect:"XPlat Code Coverage"
```

:::

## Unit Tests

### Naming Convention

```
[MethodName]_[Scenario]_[ExpectedResult]
```

Examples:
- `SerializeToJson_ThenDeserialize_ShouldPreserveSelectedFields`
- `CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException`

### Basic Test Example

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
            Hardware = new HardwareData
            {
                ComputerName = "TEST-PC",
                CpuBrand = "Intel Core i7"
            }
        };

        // Act
        var json = service.SerializeToJson(data);
        var deserialized = service.DeserializeFromJson(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal("TEST-PC", deserialized!.Hardware.ComputerName);
    }
}
```

### Theory - Parameterized Tests

```csharp
[Theory]
[InlineData(0, "Unknown")]
[InlineData(90, "Excellent")]
[InlineData(75, "Good")]
[InlineData(60, "Fair")]
public void GetHealthGrade_ShouldReturnCorrectGrade(int score, string expected)
{
    var grade = PerformanceService.GetHealthGrade(score);
    Assert.Equal(expected, grade);
}
```

### Using Stubs

```csharp
private sealed class StubHardwareService : IHardwareService
{
    private readonly HardwareData _data;
    
    public StubHardwareService(HardwareData data) => _data = data;
    
    public HardwareData GetHardwareInfo(CancellationToken ct = default) 
        => _data;
}
```

### Async Tests

```csharp
[Fact]
public async Task CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException()
{
    using var cts = new CancellationTokenSource();
    cts.Cancel();
    
    var service = CreateService();
    
    await Assert.ThrowsAsync<OperationCanceledException>(
        () => service.CollectAsync(7, cancellationToken: cts.Token));
}
```

## Property-Based Tests

```csharp
public class ReportServicePropertyTests
{
    [Property(MaxTest = 100)]
    public void SerializeDeserialize_RoundTripPreservesData(
        NonEmptyString computerName,
        NonNegativeInt cpuCores)
    {
        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                CpuCores = (uint)(cpuCores.Get % 128)
            }
        };
        
        var service = new ReportService();
        var json = service.SerializeToJson(data);
        var result = service.DeserializeFromJson(json);
        
        Assert.NotNull(result);
        Assert.Equal(data.Hardware.ComputerName, result!.Hardware.ComputerName);
    }
}
```

## Best Practices

1. **Test Isolation**: Each test creates fresh instances
2. **Boundary Testing**: Test edge cases and extreme values
3. **Exception Paths**: Test error handling
4. **Avoid Over-Mocking**: Use real objects for simple logic

See our [Contributing Guide](/en-US/guide/contributing) for more details on code standards.
