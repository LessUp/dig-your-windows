# 移除 Rust 模块 & C# 架构重构

**日期**: 2025-12-14
**版本**: 0.2.0
**类型**: Major Refactor

---

## 概述

本版本是一个重大里程碑，采用全 C# 方案（WPF + C# 采集/分析），不再维护 Rust 模块。

---

## 架构决策

| 决策 | 说明 |
|------|------|
| 移除 Rust | 统一技术栈为 .NET 生态 |
| 引入 DI | 组合根在 `App.xaml.cs` |
| 框架升级 | 目标 `net10.0-windows` |

---

## 新增功能

### 依赖注入架构

```csharp
// App.xaml.cs
protected override void OnStartup(StartupEventArgs e)
{
    var services = new ServiceCollection();
    ConfigureServices(services);
    _serviceProvider = services.BuildServiceProvider();

    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
    mainWindow.Show();
}
```

### JSON 导出/导入

- `ExportToJsonCommand`: 导出 `DiagnosticData` 到桌面
- `ImportFromJsonCommand`: 从 JSON 文件加载到界面

### 主题切换

- 深色/浅色主题一键切换
- 使用 WPF-UI `ApplicationThemeManager`

### 实时监控

- CPU 温度/负载/频率
- GPU 温度/负载/显存
- 网络流量（下载/上传）

### 可靠性趋势

- ScottPlot 图表可视化
- 按日期和类型分类

---

## 统一数据契约

| 新模型 | 替代 |
|--------|------|
| `HardwareData` | `HardwareInfo` |
| `GpuInfoData` | 合并到统一模型 |
| `LogEventData` | `EventLogEntry` |
| `ReliabilityRecordData` | `ReliabilityRecord` |
| `PerformanceAnalysisData` | `PerformanceAnalysis` |

---

## 移除的内容

| 类型 | 内容 |
|------|------|
| 模块 | `DigYourWindows/DigYourWindows_Rust/` |
| 模型 | `HardwareInfo.cs`, `PerformanceAnalysis.cs`, `EventLogEntry.cs`, `ReliabilityRecord.cs` |
| 配置 | `StartupUri` (改为 DI 创建窗口) |

---

## 修复

| 问题 | 修复 |
|------|------|
| 异步加载线程安全 | 后台线程采集，UI 线程更新 |
| `GpuMonitorService` 生命周期 | 由 DI 容器统一释放 |
| 编译错误 CS0102 | `ReportException` 静态方法与属性同名冲突 |
| 编译错误 CS0200 | `WmiException.Query` 只读问题 |

---

## 影响

- 仓库仅保留 C# WPF 版本
- 后续演进以 .NET 生态为主
- 更好的 Windows 平台集成
