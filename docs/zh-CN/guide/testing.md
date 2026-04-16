# 测试指南

本文档介绍 DigYourWindows 的测试策略、框架选择和最佳实践。

## 测试策略概览

```
        ┌─────────────────────────────────────────┐
        │          DigYourWindows 测试策略         │
        └─────────────────────────────────────────┘
                          │
        ┌─────────────────┼─────────────────┐
        ▼                 ▼                 ▼
   ┌─────────┐     ┌─────────────┐    ┌──────────┐
   │ 单元测试 │     │  属性测试   │    │ 集成测试 │
   │  xUnit  │     │  FsCheck   │    │ （预留） │
   └─────────┘     └─────────────┘    └──────────┘
        │                 │                 │
        ▼                 ▼                 ▼
   服务逻辑测试      数据不变性测试      端到端测试
   边界条件验证      随机输入测试        工作流验证
   异常路径测试      反例搜索            性能测试
```

## 测试框架

| 依赖 | 版本 | 用途 |
|------|------|------|
| xUnit | 2.9.2 | 单元测试框架 |
| FsCheck | 2.16.6 | 属性测试框架 |
| FsCheck.Xunit | 2.16.6 | xUnit 集成 |
| Microsoft.NET.Test.Sdk | 17.11.1 | 测试运行器 |
| coverlet.collector | 6.0.2 | 代码覆盖率 |

## 项目结构

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

::: code-group

```powershell [运行所有测试]
dotnet test DigYourWindows.slnx
```

```powershell [Release 模式]
dotnet test DigYourWindows.slnx -c Release
```

```powershell [详细输出]
dotnet test --logger "console;verbosity=detailed"
```

:::

### 过滤测试

```powershell
# 按类名过滤
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# 按方法名过滤
dotnet test --filter "FullyQualifiedName~SerializeToJson_ThenDeserialize"

# 按类别过滤（需要 Trait 特性）
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Property"
```

### 代码覆盖率

```powershell
# 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 生成 HTML 报告（需要 reportgenerator 工具）
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator 
  -reports:**/coverage.cobertura.xml 
  -targetdir:coverage_report
```

覆盖率报告位置：`tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml`

## 单元测试

### 测试命名规范

```
[MethodName]_[Scenario]_[ExpectedResult]
```

**示例**:
- `SerializeToJson_ThenDeserialize_ShouldPreserveSelectedFields`
- `CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException`

### Assert 选择指南

| 场景 | 推荐 Assert | 示例 |
|------|-------------|------|
| 相等判断 | `Assert.Equal` | `Assert.Equal(expected, actual)` |
| Null 检查 | `Assert.NotNull` | `Assert.NotNull(result)` |
| 异常抛出 | `Assert.Throws` | `Assert.Throws<ArgumentException>(() => ...)` |
| 集合包含 | `Assert.Contains` | `Assert.Contains(expected, collection)` |
| 布尔判断 | `Assert.True/False` | `Assert.True(result.IsValid)` |
| 范围判断 | `Assert.InRange` | `Assert.InRange(score, 0, 100)` |
| 类型判断 | `Assert.IsType` | `Assert.IsType<ReportException>(ex)` |

### 基本测试示例

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
                CpuBrand = "Intel Core i7",
                CpuCores = 8,
                TotalMemory = 17179869184 // 16GB
            },
            CollectedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var json = service.SerializeToJson(data, indented: false);
        var deserialized = service.DeserializeFromJson(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal("TEST-PC", deserialized!.Hardware.ComputerName);
        Assert.Equal("Intel Core i7", deserialized.Hardware.CpuBrand);
        Assert.Equal(8U, deserialized.Hardware.CpuCores);
        Assert.Equal(17179869184UL, deserialized.Hardware.TotalMemory);
    }
}
```

### Theory - 参数化测试

```csharp
public class PerformanceServiceTests
{
    [Theory]
    [InlineData(0, "未知")]      // 边界：0 分
    [InlineData(-1, "未知")]     // 无效：负数
    [InlineData(90, "优秀")]     // 边界：90 分
    [InlineData(89, "良好")]     // 边界：89 分
    [InlineData(75, "良好")]     // 边界：75 分
    [InlineData(74, "一般")]     // 边界：74 分
    [InlineData(60, "一般")]     // 边界：60 分
    [InlineData(59, "较差")]     // 边界：59 分
    [InlineData(40, "较差")]     // 边界：40 分
    [InlineData(39, "需要优化")] // 边界：39 分
    public void GetHealthGrade_ShouldReturnCorrectGrade(int score, string expectedGrade)
    {
        // Act
        var grade = PerformanceService.GetHealthGrade(score);

        // Assert
        Assert.Equal(expectedGrade, grade);
    }
}
```

### 使用 Stub/Mock

```csharp
public class DiagnosticCollectorServiceTests
{
    // Stub：提供预设返回值
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

    // Spy：记录调用信息
    private sealed class SpyLogService : ILogService
    {
        public List<(string Message, Exception? Exception)> Errors { get; } = new();
        public List<string> Warnings { get; } = new();

        public void Info(string message) { }
        public void Warn(string message) => Warnings.Add(message);
        public void LogError(string message, Exception? exception = null)
            => Errors.Add((message, exception));
    }

    [Fact]
    public async Task CollectAsync_WhenHardwareServiceFails_ShouldLogError()
    {
        // Arrange
        var spyLog = new SpyLogService();
        var stubHardware = new StubHardwareService(_ => 
            throw new WmiException("Access denied"));
        
        var service = new DiagnosticCollectorService(
            stubHardware, 
            Mock.Of<IEventLogService>(),
            Mock.Of<IReliabilityService>(),
            Mock.Of<IPerformanceService>(),
            spyLog);

        // Act
        await service.CollectAsync(daysBack: 7);

        // Assert
        Assert.Single(spyLog.Errors);
        Assert.Contains("Access denied", spyLog.Errors[0].Message);
    }
}
```

### 异步测试

```csharp
public class AsyncTests
{
    [Fact]
    public async Task CollectAsync_WhenSuccessful_ShouldReturnResult()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.CollectAsync(daysBack: 7);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CollectAsync_WhenCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel(); // 立即取消
        
        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => service.CollectAsync(7, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task CollectAsync_WhenCanceledMidway_ShouldReportProgress()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var progressReports = new List<DiagnosticCollectionProgress>();
        var progress = new Progress<DiagnosticCollectionProgress>(
            p => progressReports.Add(p));
        
        // 在第一次进度报告后取消
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));
        
        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => service.CollectAsync(7, progress, cts.Token));
        
        Assert.NotEmpty(progressReports);
    }
}
```

## 属性测试

### 配置

```csharp
// FsCheckConfig.cs
public static class FsCheckConfig
{
    public static Configuration Default => new Configuration
    {
        MaxNbOfTest = 100,    // 最少 100 次迭代
        StartSize = 1,        // 起始输入大小
        EndSize = 100,        // 结束输入大小
        QuietOnSuccess = true // 成功时静默
    };
}

[AttributeUsage(AttributeTargets.Method)]
public class PropertyTestAttribute : PropertyAttribute
{
    public PropertyTestAttribute()
    {
        MaxTest = 100;
        QuietOnSuccess = true;
    }
}
```

### 基本属性测试

```csharp
public class ReportServicePropertyTests
{
    [PropertyTest]
    public void SerializeDeserialize_RoundTripPreservesHardwareCoreFields(
        NonEmptyString computerName,  // 非空字符串生成器
        NonEmptyString osVersion,
        NonEmptyString cpuBrand,
        NonNegativeInt cpuCoresRaw,   // 非负整数生成器
        NonNegativeInt memoryGBRaw)
    {
        // Arrange - 限制范围避免溢出
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

        // Act
        var service = new ReportService();
        var json = service.SerializeToJson(data, indented: false);
        var deserialized = service.DeserializeFromJson(json);

        // Assert - 属性：序列化往返应保持核心字段不变
        Assert.NotNull(deserialized);
        Assert.Equal(data.Hardware.ComputerName, deserialized!.Hardware.ComputerName);
        Assert.Equal(data.Hardware.OsVersion, deserialized.Hardware.OsVersion);
        Assert.Equal(data.Hardware.CpuBrand, deserialized.Hardware.CpuBrand);
        Assert.Equal(data.Hardware.CpuCores, deserialized.Hardware.CpuCores);
        Assert.Equal(data.Hardware.TotalMemory, deserialized.Hardware.TotalMemory);
    }
}
```

### 自定义生成器

```csharp
public class CustomGenerators
{
    // 自定义健康数据生成器
    public static Arbitrary<HealthScore> HealthScoreGenerator()
    {
        return Gen.Choose(0, 100)
            .Select(score => new HealthScore(score))
            .ToArbitrary();
    }

    // 使用自定义生成器
    [Property(Arbitrary = new[] { typeof(CustomGenerators) })]
    public void HealthScore_ShouldAlwaysBeInValidRange(HealthScore score)
    {
        Assert.InRange(score.Value, 0, 100);
    }
}
```

## 最佳实践

### 1. 测试隔离

每个测试应该独立运行，不依赖其他测试的状态：

```csharp
// ✅ 好：每个测试创建新实例
[Fact]
public void Test1()
{
    var service = new ReportService();  // 新实例
    // ...
}

// ❌ 坏：共享状态可能导致测试失败
private readonly ReportService _service = new();  // 不要这样做！
```

### 2. 测试边界条件

```csharp
[Theory]
[InlineData(double.MinValue)]  // 极端小值
[InlineData(double.MaxValue)]  // 极端大值
[InlineData(double.NaN)]       // NaN
[InlineData(double.PositiveInfinity)]
[InlineData(double.NegativeInfinity)]
public void CalculateScore_WithExtremeValues_ShouldHandleGracefully(double input)
{
    // 测试极端输入的处理
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

[Fact]
public void DeserializeFromJson_WithInvalidJson_ShouldThrowReportException()
{
    var service = new ReportService();
    
    Assert.Throws<ReportException>(() => 
        service.DeserializeFromJson("not valid json"));
}
```

### 4. 避免过度 Mock

对于简单的数据转换，优先使用真实对象：

```csharp
// ✅ 好：直接使用真实对象
var service = new ReportService();

// ❌ 坏：过度 Mock 简单逻辑
var mockService = new Mock<IReportService>();
mockService.Setup(s => s.SerializeToJson(It.IsAny<DiagnosticData>()))
           .Returns("{}");
```

### 5. 有意义的测试名称

```csharp
// ✅ 好：描述行为和期望
[Fact]
public void SerializeToJson_WhenIndented_ShouldIncludeLineBreaks()

// ❌ 坏：只描述操作
[Fact]
public void TestSerializeToJson()
```

## 测试覆盖要求

| 需求 ID | 说明 | 测试类型 | 优先级 |
|---------|------|----------|--------|
| T-001 | JSON 序列化往返 | 属性测试 | P0 |
| T-002 | HTML 报告生成 | 单元测试 | P0 |
| T-003 | 健康评分计算 | 单元测试 | P0 |
| T-004 | 采集取消操作 | 单元测试 | P0 |
| T-005 | 边界条件处理 | 单元测试 | P1 |
| T-006 | 异常路径 | 单元测试 | P1 |

## CI 集成

测试在 CI 流水线中自动运行：

```yaml
# .github/workflows/ci.yml
- name: Test
  run: dotnet test DigYourWindows.slnx -c Release --no-restore --verbosity normal
  
- name: Test with Coverage
  run: dotnet test --collect:"XPlat Code Coverage" --no-build
  
- name: Upload Coverage
  uses: codecov/codecov-action@v3
  with:
    files: tests/**/coverage.cobertura.xml
```

## 调试测试

### 在 VS Code 中调试

```json
// .vscode/launch.json
{
  "name": "Debug Tests",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}/tests/DigYourWindows.Tests/bin/Debug/net10.0/DigYourWindows.Tests.dll",
  "args": [],
  "cwd": "${workspaceFolder}/tests/DigYourWindows.Tests",
  "console": "internalConsole",
  "stopAtEntry": false
}
```

### 运行单个测试

```powershell
# 在 Test Explorer 中右键点击测试
# 或使用命令行过滤
dotnet test --filter "FullyQualifiedName~ReportServiceTests.SerializeToJson"
```
