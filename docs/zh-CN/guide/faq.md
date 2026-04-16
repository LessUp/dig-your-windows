# 常见问题 (FAQ)

快速查找解决方案。使用 <kbd>Ctrl</kbd> + <kbd>F</kbd> 搜索关键词。

## 🚀 安装与运行 {#installation}

### 运行时提示"需要管理员权限"？

**A**: 以下功能需要管理员权限：
- GPU 温度/负载监控
- 磁盘 SMART 数据读取
- 部分硬件信息采集

**解决方案**:
1. 右键点击应用程序，选择"以管理员身份运行"
2. 或在 VS Code 的 `.vscode/launch.json` 中配置 `"runAsAdministrator": true`

:::

### 启动时提示缺少 .NET Runtime？

**A**: 框架依赖版本（FDD）需要安装 .NET 10 Runtime。

**解决方案**:

| 方式 | 步骤 |
|------|------|
| **方式一** | 下载自包含版本（SCD），无需安装 .NET |
| **方式二** | 从 [Microsoft 官网](https://dotnet.microsoft.com/download) 安装 .NET Desktop Runtime 10 |

### Windows Defender 报告应用为病毒？

**A**: 这是误报，因为应用使用了 WMI 和硬件监控 API。

**解决方案**:
1. 在 Windows Defender 中添加排除项
2. 使用 [VirusTotal](https://www.virustotal.com) 验证安装包
3. 如需临时使用：关闭实时保护 → 运行应用 → 重新开启保护

## 🔧 功能相关 {#features}

### 为什么 GPU 温度显示为 N/A？

**A**: 可能的原因：

| 原因 | 解决方案 |
|------|----------|
| 未以管理员身份运行 | 以管理员身份运行应用 |
| GPU 驱动不支持 | 更新显卡驱动程序 |
| 虚拟机环境 | 虚拟机中的 GPU 可能不支持监控 |
| LibreHardwareMonitor 不支持 | 检查 [兼容性列表](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) |

### SMART 数据显示不完整？

**A**: 部分磁盘控制器或外接硬盘可能不支持 SMART。

**支持的磁盘类型**:
- ✅ SATA SSD/HDD
- ✅ NVMe SSD
- ⚠️ 部分外接 USB 硬盘（取决于控制器芯片）

### 可靠性记录为什么是空的？

**A**: 可能的原因：

1. Windows 可靠性监视器数据已被清理
2. 系统刚重装不久，没有历史记录
3. 查询时间范围设置过小

**解决方案**:
- 调整"回溯天数"参数（默认 7 天，最大 90 天）
- 检查"可靠性监视器"控制面板是否有数据
- 确认事件日志服务正在运行

### 导出的 HTML 报告打开很慢？

**A**: 大量事件日志会导致 HTML 文件较大。

**优化方案**:
1. 减少"最大事件数"设置（默认 1000 条）
2. 使用 JSON 格式导出（更小，更快）
3. 使用现代浏览器打开（Chrome/Edge/Firefox）
4. 减少回溯天数范围

## 🐛 故障排除 {#troubleshooting}

### 应用启动后闪退？

**A**: 按以下步骤排查：

**步骤 1**: 查看日志
```
%LOCALAPPDATA%\DigYourWindows\Logs\DigYourWindows_YYYYMMDD.log
```

**步骤 2**: 常见问题及解决
| 症状 | 可能原因 | 解决方案 |
|------|----------|----------|
| 启动后立即退出 | .NET 版本不匹配 | 重新安装 .NET 10 SDK |
| 加载界面卡死 | 硬件监控初始化失败 | 以管理员身份运行 |
| 内存不足异常 | 系统内存不足 | 关闭其他应用程序 |

**步骤 3**: 重置应用
```powershell
# 删除配置文件（会重置所有设置）
Remove-Item -Path "$env:LOCALAPPDATA\DigYourWindows\settings.json" -Force
```

### CPU/GPU 监控数值不更新？

**A**: 监控服务可能未正确初始化。

**排查步骤**:
1. 以管理员身份重启应用
2. 检查日志中的错误信息（搜索 "HardwareMonitor"）
3. 确认 LibreHardwareMonitor 依赖正常加载
4. 尝试禁用并重新启用监控（点击刷新按钮）

### 导入 JSON 文件失败？

**A**: JSON 文件格式可能不正确或版本不兼容。

**解决方案**:
1. 确认 JSON 文件来自相同或兼容版本
2. 使用文本编辑器检查 JSON 格式（验证工具：[JSONLint](https://jsonlint.com)）
3. 查看日志中的具体错误信息
4. 检查文件是否完整（未被截断或损坏）

### 构建项目时出现大量警告？

**A**: 首次构建时可能会出现 NuGet 包还原警告，这是正常现象。

**解决方案**:
```powershell
# 完整清理并重新构建
dotnet clean
dotnet restore --force
dotnet build
```

## 💻 开发相关 {#development}

### 如何调试硬件监控功能？

**A**: 使用以下方法调试：

**方法 1: Visual Studio 调试**
1. 以管理员身份启动 Visual Studio
2. 在 `CpuMonitorService.cs` 或 `GpuMonitorService.cs` 设置断点
3. 按 F5 启动调试

**方法 2: VS Code 配置**
在 `.vscode/launch.json` 中配置：
```json
{
  "name": "Debug Hardware",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
  "windows": { "runAsAdministrator": true }
}
```

### 测试覆盖率报告在哪里？

**A**: 覆盖率报告生成步骤：

```powershell
# 1. 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 2. 报告位置
tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml

# 3. 生成 HTML 报告（可选）
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage_report
```

### 如何添加新的硬件监控指标？

**A**: 添加新指标的步骤：

1. **在 `HardwareMonitorProvider` 中启用硬件类型**
```csharp
_computer = new Computer
{
    IsCpuEnabled = true,
    IsGpuEnabled = true,
    // 添加新的硬件类型
    IsMotherboardEnabled = true,
};
```

2. **在对应的服务中添加采集逻辑**
```csharp
// 例如在 MotherboardMonitorService.cs
public MotherboardData GetMotherboardData()
{
    var motherboard = _provider.Computer.Hardware
        .FirstOrDefault(h => h.HardwareType == HardwareType.Motherboard);
    // 读取传感器数据...
}
```

3. **更新数据模型**
```csharp
// 在 HardwareData.cs 中添加新属性
public MotherboardData? Motherboard { get; set; }
```

4. **添加单元测试**
```csharp
[Fact]
public void GetMotherboardData_ShouldReturnValidData()
{
    // 测试逻辑...
}
```

### 如何修改健康评分算法？

**A**: 健康评分算法在 `PerformanceService.cs` 中：

```csharp
// 修改权重配置
private static class Weights
{
    public const double Stability = 0.40;   // 稳定性 40%
    public const double Performance = 0.30; // 性能 30%
    public const double Memory = 0.15;      // 内存 15%
    public const double Disk = 0.15;        // 磁盘 15%
}

// 修改评分逻辑
private double CalculateStabilityScore(...)
{
    // 自定义评分逻辑
}
```

## 🌐 其他问题 {#other}

### 支持哪些操作系统？

**A**:

| 操作系统 | 支持状态 | 说明 |
|----------|----------|------|
| Windows 11 | ✅ 完全支持 | 推荐 |
| Windows 10 (Build 19041+) | ✅ 完全支持 | 需要 20H1 或更高版本 |
| Windows 10 (Build < 19041) | ⚠️ 有限支持 | 部分功能可能不可用 |
| Windows 7/8 | ❌ 不支持 | .NET 10 不支持 |
| Linux/macOS | ❌ 不支持 | Windows 专用工具 |

### 数据会上传到云端吗？

**A**: **不会**。所有数据均在本地采集、存储和导出：

- ✅ 数据保存在本地文件系统
- ✅ 导出文件由用户控制
- ✅ 应用不会连接任何远程服务器
- ✅ 开源代码，可审计

### 如何获取最新版本？

**A**:

**方式 1**: GitHub Releases（推荐）
```
https://github.com/LessUp/dig-your-windows/releases/latest
```

**方式 2**: 自动更新（计划支持）
未来版本将支持检查更新并自动下载。

**方式 3**: 源码构建
```powershell
git pull
dotnet build
dotnet run
```

### 如何反馈问题或建议？

**A**:

| 渠道 | 用途 | 链接 |
|------|------|------|
| GitHub Issues | Bug 报告、功能请求 | [创建 Issue](https://github.com/LessUp/dig-your-windows/issues/new/choose) |
| GitHub Discussions | 一般讨论、问题解答 | [进入讨论区](https://github.com/LessUp/dig-your-windows/discussions) |
| 电子邮件 | 敏感问题、商业合作 | lessup@example.com |

**提交 Issue 时建议包含**:
- 操作系统版本
- .NET 版本 (`dotnet --version`)
- 应用版本（在关于页面查看）
- 复现步骤
- 相关日志片段

## 📋 快速参考 {#reference}

### 快捷键

| 快捷键 | 功能 |
|--------|------|
| <kbd>F5</kbd> | 刷新诊断数据 |
| <kbd>Ctrl</kbd> + <kbd>S</kbd> | 保存报告 |
| <kbd>Ctrl</kbd> + <kbd>T</kbd> | 切换主题 |
| <kbd>Esc</kbd> | 取消正在进行的采集 |

### 日志级别

| 级别 | 用途 | 示例 |
|------|------|------|
| INFO | 一般信息 | 采集开始/完成 |
| WARN | 警告 | 权限不足，功能降级 |
| ERROR | 错误 | 服务初始化失败 |

### 健康评分等级

| 分数 | 等级 | 颜色 | 说明 |
|------|------|------|------|
| 90-100 | 优秀 | 🟢 | 系统状态良好 |
| 75-89 | 良好 | 🔵 | 系统状态正常 |
| 60-74 | 一般 | 🟡 | 存在一些问题 |
| 40-59 | 较差 | 🟠 | 需要关注 |
| < 40 | 需要优化 | 🔴 | 需要立即处理 |

---

**没有找到答案？** [创建 Issue](https://github.com/LessUp/dig-your-windows/issues/new) 或查看 [完整文档](/zh-CN/guide/)。
