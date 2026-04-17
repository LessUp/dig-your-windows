# 测试指南

本文档介绍 DigYourWindows 的测试策略和最佳实践。

## 测试框架

| 依赖 | 版本 | 用途 |
|------|------|------|
| xUnit | 2.9.2 | 测试框架 |
| FsCheck | 2.16.6 | 属性测试框架 |
| FsCheck.Xunit | 2.16.6 | xUnit 集成 |
| Microsoft.NET.Test.Sdk | 17.11.1 | 测试运行器 |
| coverlet.collector | 6.0.2 | 代码覆盖率 |

## 测试结构

```
DigYourWindows.Tests/
├── Unit/                           # 单元测试
│   ├── ReportServiceTests.cs       # 报告服务测试
│   ├── DiagnosticCollectorServiceTests.cs
│   └── PerformanceServiceTests.cs  # 性能分析测试
├── Property/                       # 属性测试
│   └── ReportServicePropertyTests.cs
├── Integration/                    # 集成测试（预留）
├── FsCheckConfig.cs               # FsCheck 配置
├── Usings.cs                      # 全局 using
└── DigYourWindows.Tests.csproj
```

## 运行测试

### 基本命令

```powershell
# 运行所有测试
dotnet test DigYourWindows.slnx

# Release 模式
dotnet test DigYourWindows.slnx -c Release

# 详细输出
dotnet test --logger "console;verbosity=detailed"
```

### 过滤测试

```powershell
# 按类名过滤
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# 按方法名过滤
dotnet test --filter "FullyQualifiedName~SerializeToJson_ThenDeserialize"

# 按类别过滤
dotnet test --filter "Category=Unit"
```

### 代码覆盖率

```powershell
# 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 生成报告（需要 reportgenerator 工具）
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

## 单元测试

### 测试命名规范

```
[MethodName]_[Scenario]_[ExpectedResult]
```

示例：
- `SerializeToJson_ThenDeserialize_ShouldPreserveSelectedFields`
- `CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException`

### 测试示例

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

### 使用 Stub/Mock

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

## 属性测试

### 配置

`FsCheckConfig.cs` 定义了默认配置：

```csharp
public static class FsCheckConfig
{
    public static Configuration Default => new Configuration
    {
        MaxNbOfTest = 100,    // 最少 100 次迭代
        QuietOnSuccess = true
    };
}

// 自定义特性
public class PropertyTestAttribute : PropertyAttribute
{
    public PropertyTestAttribute()
    {
        MaxTest = 100;
        QuietOnSuccess = true;
    }
}
```

### 测试示例

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

## 测试覆盖的需求

| 需求 ID | 说明 | 测试类型 |
|---------|------|----------|
| 2.1 | Schema 验证和数据解析 | 单元测试 |
| 2.2 | HTML 报告生成验证 | 单元测试 |
| 2.3 | 性能评分范围验证 | 单元测试 |
| 2.4 | 畸形输入处理 | 单元测试 |
| 2.5 | JSON 序列化往返 | 属性测试 |

## 最佳实践

### 1. 测试隔离

每个测试应该独立运行，不依赖其他测试的状态：

```csharp
// Good: 每个测试创建新实例
[Fact]
public void Test1()
{
    var service = new ReportService();  // 新实例
    // ...
}
```

### 2. 测试边界条件

```csharp
[Theory]
[InlineData(0, "未知")]
[InlineData(-1, "未知")]
[InlineData(90, "优秀")]
[InlineData(89, "良好")]
[InlineData(50, "一般")]
public void GetHealthGrade_ShouldReturnCorrectGrade(int score, string expectedGrade)
{
    // Test boundary conditions
}
```

### 3. 测试异常路径

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

### 4. 避免过度 Mock

对于简单的数据转换，优先使用真实对象：

```csharp
// Good: 直接使用真实 ReportService
var service = new ReportService();

// 避免: 过度 Mock 简单逻辑
var mockService = new Mock<IReportService>();
mockService.Setup(s => s.SerializeToJson(It.IsAny<DiagnosticData>()))
           .Returns("{}");
```

## CI 集成

测试在 CI 流水线中自动运行：

```yaml
# .github/workflows/ci.yml
- name: Test
  run: dotnet test DigYourWindows.slnx -c Release --no-restore
```
