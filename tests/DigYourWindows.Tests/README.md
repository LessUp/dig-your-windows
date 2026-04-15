# DigYourWindows Tests

测试项目包含单元测试和属性测试，确保核心业务逻辑的正确性。

## 目录结构

```
DigYourWindows.Tests/
├── Unit/                           # 单元测试
│   ├── ReportServiceTests.cs       # 报告服务测试
│   ├── DiagnosticCollectorServiceTests.cs  # 诊断采集测试
│   └── PerformanceServiceTests.cs  # 性能分析测试
├── Property/                       # 属性测试
│   └── ReportServicePropertyTests.cs
├── Integration/                    # 集成测试（预留）
├── FsCheckConfig.cs               # FsCheck 配置
├── Usings.cs                      # 全局 using
└── DigYourWindows.Tests.csproj
```

## 测试框架

| 依赖 | 版本 | 用途 |
|------|------|------|
| xUnit | 2.9.2 | 测试框架 |
| FsCheck | 2.16.6 | 属性测试 |
| FsCheck.Xunit | 2.16.6 | xUnit 集成 |
| Microsoft.NET.Test.Sdk | 17.11.1 | 测试 SDK |
| coverlet.collector | 6.0.2 | 代码覆盖率 |

## 运行测试

### 运行所有测试

```powershell
dotnet test
```

### 详细输出

```powershell
dotnet test --logger "console;verbosity=detailed"
```

### 运行特定测试

```powershell
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

### 代码覆盖率

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

## 属性测试

使用 FsCheck 进行属性测试，每个属性测试默认运行 100 次迭代。

### 配置

```csharp
// FsCheckConfig.cs
public static class FsCheckConfig
{
    public static Configuration Default => new Configuration
    {
        MaxNbOfTest = 100,
        QuietOnSuccess = true
    };
}
```

### 自定义特性

```csharp
[PropertyTest]  // 自动应用 100 次迭代配置
public void MyPropertyTest(int x)
{
    Assert.True(x >= int.MinValue);
}
```

## 测试覆盖的需求

| 需求 | 说明 | 测试类型 |
|------|------|----------|
| 2.1 | Schema 验证和数据解析 | 单元测试 |
| 2.2 | HTML 报告生成验证 | 单元测试 |
| 2.3 | 性能评分范围验证 | 单元测试 |
| 2.4 | 畸形输入处理 | 单元测试 |
| 2.5 | JSON 序列化往返 | 属性测试 |

## 最佳实践

1. **测试隔离** - 每个测试应独立运行
2. **命名规范** - `[MethodName]_[Scenario]_[ExpectedResult]`
3. **边界测试** - 使用 `[Theory]` 测试边界条件
4. **异常路径** - 测试错误和异常情况
5. **避免过度 Mock** - 对简单逻辑使用真实对象
