# 贡献指南

感谢你对 DigYourWindows 项目的兴趣！本文档将帮助你参与项目开发。

## 行为准则

- 尊重所有贡献者，保持友善和专业
- 接受建设性批评，乐于学习
- 关注对社区最有利的事情
- 遵守开源社区的道德规范

## 快速开始

### 开发环境检查清单

在开始贡献之前，请确保你的环境满足以下要求：

- [ ] Windows 10/11 (Build 19041+)
- [ ] .NET 10.0 SDK 已安装 (`dotnet --version` 应显示 10.0.x)
- [ ] Git 已安装并配置
- [ ] IDE 已安装（VS 2022、Rider 或 VS Code）
- [ ] GitHub 账号已创建
- [ ] 已 Fork 本仓库

### 设置开发环境

```powershell
# 1. Fork 仓库（在 GitHub 网页上点击 Fork 按钮）

# 2. 克隆你的 Fork
git clone https://github.com/YOUR_USERNAME/dig-your-windows.git
cd dig-your-windows

# 3. 添加上游仓库
git remote add upstream https://github.com/LessUp/dig-your-windows.git

# 4. 还原依赖
dotnet restore DigYourWindows.slnx

# 5. 验证构建
dotnet build DigYourWindows.slnx

# 6. 运行测试
dotnet test DigYourWindows.slnx

# 7. 运行应用
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

## 贡献类型

### 报告 Bug

1. **搜索现有 Issue**
   在 [Issues](https://github.com/LessUp/dig-your-windows/issues) 中搜索，确认问题未被报告

2. **收集信息**
   - 操作系统版本：`(Get-WmiObject -class Win32_OperatingSystem).Caption`
   - .NET 版本：`dotnet --version`
   - 应用版本：在"关于"页面查看
   - 复现步骤
   - 预期行为与实际行为
   - 相关日志（位于 `%LOCALAPPDATA%\DigYourWindows\Logs\`）

3. **创建 Issue**
   使用 [Bug 报告模板](https://github.com/LessUp/dig-your-windows/issues/new?template=bug_report.md) 创建新 Issue

### 提交功能请求

1. **搜索现有请求**
   检查是否已有类似的功能请求

2. **描述功能需求**
   - 清晰描述功能需求和用例
   - 解释该功能如何使项目受益
   - 如果可能，提供实现思路或参考

3. **创建 Issue**
   使用 [功能请求模板](https://github.com/LessUp/dig-your-windows/issues/new?template=feature_request.md) 创建新 Issue

### 提交代码

#### 分支策略

```
upstream/master
    │
    ├── feature/hardware-monitoring-improvements
    │   └── PR #123 [已合并]
    │
    ├── bugfix/memory-leak-in-logservice
    │   └── PR #124 [已合并]
    │
    └── release/v1.1.0 [标签 v1.1.0]
```

#### 工作流

```powershell
# 1. 同步上游仓库
git checkout master
git fetch upstream
git merge upstream/master
git push origin master

# 2. 创建功能分支
git checkout -b feature/your-feature-name
# 或
git checkout -b bugfix/issue-description

# 3. 进行修改并提交
git add .
git commit -m "feat(service): add new monitoring capability"

# 4. 保持分支同步（避免冲突）
git fetch upstream
git rebase upstream/master

# 5. 推送到你的 Fork
git push origin feature/your-feature-name

# 6. 在 GitHub 上创建 Pull Request
```

#### 分支命名规范

| 类型 | 格式 | 示例 |
|------|------|------|
| 功能 | `feature/描述` | `feature/gpu-memory-monitoring` |
| Bug 修复 | `bugfix/描述` 或 `fix/issue-编号` | `bugfix/null-ref-in-report` |
| 文档 | `docs/描述` | `docs/update-api-reference` |
| 重构 | `refactor/描述` | `refactor/simplify-collector` |
| 性能优化 | `perf/描述` | `perf/reduce-memory-allocations` |

## 代码规范

### C# 编码约定

遵循 [Microsoft C# 编码约定](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions) 和项目 `.editorconfig` 配置。

#### 命名规范

```csharp
// ✅ 类、结构体、枚举、方法：PascalCase
public class DiagnosticCollectorService { }
public void CollectData() { }

// ✅ 局部变量、参数：camelCase
var collectedData = new DiagnosticData();
void ProcessData(DiagnosticData data) { }

// ✅ 常量：PascalCase
public const int MaxRetryCount = 3;

// ✅ 接口：IPascalCase
public interface IHardwareService { }

// ✅ 私有字段：_camelCase
private readonly ILogService _log;

// ✅ 泛型类型参数：T + 描述
public class Repository<TEntity> where TEntity : class { }
```

#### 代码组织

```csharp
// 文件结构示例
using System;
using System.Collections.Generic;
using DigYourWindows.Core.Models;
using DigYourWindows.Core.Exceptions;

namespace DigYourWindows.Core.Services;

/// <summary>
/// 采集系统诊断数据的服务
/// </summary>
public sealed class DiagnosticCollectorService : IDiagnosticCollectorService
{
    // 常量
    private const int DefaultDaysBack = 7;
    
    // 字段（按访问修饰符分组）
    private readonly IHardwareService _hardwareService;
    private readonly IEventLogService _eventLogService;
    private readonly ILogService _log;
    
    // 构造函数
    public DiagnosticCollectorService(
        IHardwareService hardwareService,
        IEventLogService eventLogService,
        ILogService log)
    {
        _hardwareService = hardwareService;
        _eventLogService = eventLogService;
        _log = log;
    }
    
    // 公共方法
    public async Task<DiagnosticCollectionResult> CollectAsync(
        int daysBack = DefaultDaysBack,
        CancellationToken cancellationToken = default)
    {
        // 实现...
    }
    
    // 私有方法
    private async Task<HardwareData> CollectHardwareAsync(
        CancellationToken cancellationToken)
    {
        // 实现...
    }
}
```

#### XML 文档注释

所有公共 API 必须包含 XML 文档注释：

```csharp
/// <summary>
/// 采集系统诊断数据
/// </summary>
/// <param name="daysBack">回溯天数，用于查询事件日志和可靠性记录</param>
/// <param name="progress">进度报告回调（可选）</param>
/// <param name="cancellationToken">取消令牌</param>
/// <returns>包含采集结果的诊断数据对象</returns>
/// <exception cref="ServiceException">当服务层发生错误时抛出</exception>
/// <exception cref="OperationCanceledException">当操作被取消时抛出</exception>
public async Task<DiagnosticCollectionResult> CollectAsync(
    int daysBack = 7,
    IProgress<DiagnosticCollectionProgress>? progress = null,
    CancellationToken cancellationToken = default)
{
    // 实现...
}
```

### 提交信息规范

使用 [Conventional Commits](https://www.conventionalcommits.org/) 格式：

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

#### 类型 (Type)

| 类型 | 说明 | 示例 |
|------|------|------|
| `feat` | 新功能 | `feat(monitor): add GPU memory usage monitoring` |
| `fix` | Bug 修复 | `fix(reliability): handle empty records correctly` |
| `docs` | 文档更新 | `docs(guide): add troubleshooting section` |
| `style` | 代码格式（不影响功能）| `style: remove trailing whitespace` |
| `refactor` | 代码重构 | `refactor(service): extract helper method` |
| `perf` | 性能优化 | `perf(collector): reduce allocations` |
| `test` | 测试相关 | `test(unit): add boundary tests` |
| `chore` | 构建/工具相关 | `chore(deps): update packages` |

#### 示例

```powershell
# 功能提交
git commit -m "feat(monitor): add CPU temperature trend chart

- Add real-time temperature monitoring
- Implement 5-minute rolling average
- Update UI to display trend"

# Bug 修复
git commit -m "fix(service): handle WMI access denied exception

Previously the application would crash when WMI returned
access denied. Now it logs a warning and continues with
available data.

Fixes #123"

# 文档更新
git commit -m "docs(readme): update installation instructions

- Add PowerShell Core instructions
- Clarify .NET SDK requirement
- Add screenshots"
```

## Pull Request 流程

### PR 检查清单

在提交 PR 之前，请确保：

- [ ] 代码遵循项目编码规范
- [ ] 已添加必要的单元测试/属性测试
- [ ] 所有测试通过 (`dotnet test`)
- [ ] 已更新相关文档
- [ ] 提交信息符合 Conventional Commits 规范
- [ ] 已同步上游最新代码（无冲突）
- [ ] PR 描述清晰，关联相关 Issue

### PR 描述模板

```markdown
## 描述
简要描述这个 PR 做了什么

## 类型
- [ ] Bug 修复
- [ ] 新功能
- [ ] 性能优化
- [ ] 文档更新
- [ ] 代码重构
- [ ] 其他（请说明）

## 测试
- [ ] 已添加/更新单元测试
- [ ] 所有现有测试通过
- [ ] 手动测试验证完成

## 相关 Issue
Closes #123
Related to #456

## 截图（如适用）
（如果有 UI 变更，请提供截图）

## 检查清单
- [ ] 代码遵循编码规范
- [ ] 已添加必要的测试
- [ ] 已更新文档
```

### 代码审查流程

1. **自动检查**
   - CI 构建必须通过
   - 代码覆盖率不应下降
   - 静态分析不应产生新警告

2. **人工审查**
   - 至少需要 1 个维护者批准
   - 审查者会检查代码质量、测试覆盖、文档更新
   - 可能会提出修改建议

3. **合并**
   - 使用 "Squash and merge" 合并
   - 合并后会删除功能分支

## 项目结构说明

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/    # 核心业务逻辑
│   │   ├── Models/              # 数据模型（POCO）
│   │   ├── Services/            # 业务逻辑服务
│   │   └── Exceptions/          # 自定义异常
│   └── DigYourWindows.UI/       # WPF 用户界面
│       ├── ViewModels/          # MVVM 视图模型
│       ├── Converters/          # 值转换器
│       └── App.xaml.cs          # 应用入口
├── tests/
│   └── DigYourWindows.Tests/    # 测试项目
│       ├── Unit/                # 单元测试
│       ├── Property/            # 属性测试
│       └── Integration/         # 集成测试（预留）
├── docs/                        # VitePress 文档站
├── changelog/                   # 变更日志
└── scripts/                     # 构建脚本
```

## 发布流程

### 版本号管理

使用 [语义化版本](https://semver.org/lang/zh-CN/)：

- **主版本号（MAJOR）**: 不兼容的 API 变更
- **次版本号（MINOR）**: 向下兼容的功能添加
- **修订号（PATCH）**: 向下兼容的问题修复

### 发布步骤

1. **准备发布**
   - 更新 `Directory.Build.props` 中的版本号
   - 更新 `CHANGELOG.md`
   - 检查所有测试通过

2. **创建发布**
   ```powershell
   git checkout master
   git pull upstream master
   git tag v1.1.0
   git push origin v1.1.0
   git push upstream v1.1.0
   ```

3. **GitHub Actions 自动执行**
   - 构建 FDD 和 SCD 版本
   - 创建 GitHub Release

## 获取帮助

### 文档

- [项目文档](https://lessup.github.io/dig-your-windows/)
- [API 参考](/zh-CN/reference/data-schema)
- [FAQ](/zh-CN/guide/faq)

### 社区

- [GitHub Discussions](https://github.com/LessUp/dig-your-windows/discussions) - 一般讨论
- [GitHub Issues](https://github.com/LessUp/dig-your-windows/issues) - Bug 报告和功能请求

### 其他资源

- [.NET 文档](https://docs.microsoft.com/dotnet/)
- [WPF-UI 文档](https://lepoco.org/wpfui/)
- [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)

## 许可证

提交代码即表示你同意你的贡献将按照 [MIT License](LICENSE) 发布。

---

**感谢你的贡献！** 🎉
