# 测试指南

## 测试框架

| 依赖 | 版本 | 用途 |
|------|------|------|
| xUnit | 2.9.2 | 测试框架 |
| FsCheck | 2.16.6 | 属性测试 |
| FsCheck.Xunit | 2.16.6 | xUnit 集成 |
| Microsoft.NET.Test.Sdk | 17.11.1 | 测试 SDK |

## 测试结构

```
DigYourWindows.Tests/
├── Unit/                      # 单元测试
│   └── SampleUnitTests.cs
├── Property/                  # 属性测试
│   └── SamplePropertyTests.cs
├── Integration/               # 集成测试
├── FsCheckConfig.cs          # FsCheck 配置
├── Usings.cs                 # 全局 usings
└── DigYourWindows.Tests.csproj
```

## 运行测试

```powershell
# 运行全部测试
dotnet test DigYourWindows.slnx

# 详细输出
dotnet test --logger "console;verbosity=detailed"

# 过滤特定测试类
dotnet test --filter "FullyQualifiedName~SamplePropertyTests"
```

## 配置

- **属性测试迭代次数**：每个 property 最少 100 次
- **自定义特性**：`[PropertyTest]` 应用默认 FsCheck 配置
- **配置类**：`FsCheckConfig`

## 覆盖的验证需求

| 需求 | 说明 |
|------|------|
| 2.1 | Schema 验证和数据解析 |
| 2.2 | HTML 报告生成验证 |
| 2.3 | 性能评分范围验证 |
| 2.4 | 畸形输入处理 |
| 2.5 | JSON 序列化往返 |
