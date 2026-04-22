# Testing Specification

> **Domain**: testing
> **Version**: 1.0.0
> **Status**: accepted
> **Last Updated**: 2026-04-17

## Overview

本文档定义 DigYourWindows 的测试策略，包括测试类型、覆盖率要求和质量门禁。

## Testing Types

### 1. 单元测试 (Unit Tests)

**目的**：验证独立的方法和类。

**框架**：xUnit 2.9+

**覆盖范围**：
- 所有服务方法
- 所有模型验证逻辑
- 所有异常处理路径
- 所有转换器类
- 所有工具函数

**最低覆盖率**：80% 行覆盖率

**示例结构**：

```csharp
public class ReportServiceTests
{
    [Fact]
    public void ExportToJson_WithValidData_ReturnsValidJson()
    {
        // Arrange
        var service = new ReportService();
        var data = CreateTestDiagnosticData();

        // Act
        var json = service.ExportToJson(data);

        // Assert
        Assert.NotNull(json);
        Assert.ValidJson(json);
    }

    [Fact]
    public void ExportToJson_WithNullData_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => 
            service.ExportToJson(null));
    }
}
```

### 2. 属性测试 (Property-Based Tests)

**目的**：验证属性在广泛输入范围内成立。

**框架**：FsCheck 2.16+

**覆盖领域**：
- 数据验证规则
- 序列化/反序列化往返
- 数学计算（分数、百分比）
- 收集和过滤逻辑

**示例结构**：

```csharp
public class DiagnosticDataProperties
{
    [Property]
    public Property ScoreRangeIsValid(
        int stabilityScore,
        int performanceScore)
    {
        return (stabilityScore >= 0 && stabilityScore <= 100 &&
                performanceScore >= 0 && performanceScore <= 100)
            .ToProperty();
    }
}
```

### 3. 集成测试 (Integration Tests)

**目的**：验证与真实 Windows API 的端到端工作流。

**框架**：xUnit + 条件执行

**覆盖领域**：
- WMI 查询（Windows 可用时）
- 事件日志读取（权限允许时）
- 硬件监控（硬件兼容时）
- 文件系统操作

**执行**：
- 标记为 `[SkippableFact]` 属性
- 当环境不支持时优雅跳过
- 仅在 Windows CI agent 上运行

## Test Organization

### Directory Structure

```
tests/DigYourWindows.Tests/
├── Unit/                    # 单元测试
│   ├── Services/
│   ├── Models/
│   └── Converters/
├── Property/                # 属性测试
│   ├── DataValidationProperties.cs
│   └── SerializationProperties.cs
├── Integration/             # 集成测试（条件执行）
│   └── WmiIntegrationTests.cs
└── FsCheckConfig.cs         # FsCheck 配置
```

### Test Naming Convention

**模式**：`{MethodName}_{Scenario}_{ExpectedBehavior}`

**示例**：
- `ExportToJson_WithValidData_ReturnsValidJson`
- `ExportToJson_WithNullData_ThrowsException`
- `GetErrorEvents_WithEmptyLog_ReturnsEmptyCollection`

## Coverage Requirements

### 行覆盖率

| 组件 | 最低覆盖率 | 备注 |
|------|-----------|------|
| Services | 85% | 核心业务逻辑 |
| Models | 80% | 验证和序列化 |
| Converters | 90% | 简单 UI 助手 |
| Exceptions | 80% | 异常工厂方法 |
| **总体** | **80%** | 项目范围最低要求 |

### 分支覆盖率

| 组件 | 最低覆盖率 |
|------|-----------|
| Services | 75% |
| Models | 70% |
| **总体** | **70%** |

## Quality Gates

### CI Pipeline 检查

所有 pull request 必须通过：

1. **构建检查**：`dotnet build` 成功，无错误
2. **单元测试**：所有单元测试通过
3. **属性测试**：所有属性测试通过（每次 100 迭代）
4. **覆盖率检查**：代码覆盖率满足最低阈值
5. **Linting**：无新警告（TreatWarningsAsErrors=true）

### 测试执行命令

```powershell
# 运行所有测试
dotnet test

# 运行并收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 运行特定测试类别
dotnet test --filter "Category=Unit"

# 运行特定测试类
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

## Mocking Strategy

### 何时 Mock

**Mock**：
- 外部依赖（WMI, EventLog）
- 文件系统操作
- 硬件提供者（LibreHardwareMonitor）
- 时间依赖操作

**不 Mock**：
- 纯函数
- 简单数据转换
- 模型类

### Mock 实现

**推荐**：Moq 或手动 mock 实现

**示例**：

```csharp
public class MockHardwareMonitorProvider : IHardwareMonitorProvider
{
    public Computer Computer => CreateTestComputer();
}
```

## Test Data Management

### Test Data Factories

**原则**：使用工厂方法创建测试数据，而非硬编码值。

**示例**：

```csharp
public static class TestDataProvider
{
    public static DiagnosticData CreateValidDiagnosticData()
    {
        return new DiagnosticData
        {
            Timestamp = DateTime.UtcNow,
            SystemInfo = CreateValidSystemInfo(),
            HardwareData = CreateValidHardwareData()
        };
    }
}
```

## Continuous Integration

### GitHub Actions Workflow

**触发**：Push to main, pull requests

**步骤**：
1. Checkout code
2. Setup .NET 10.0
3. Restore dependencies
4. Build solution
5. Run unit tests
6. Run property tests
7. Collect coverage

## References

- [Architecture Specification](../architecture/spec.md)
- [Data Specification](../data/spec.md)
