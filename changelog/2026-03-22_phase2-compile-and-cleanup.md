# 编译恢复、流程收敛与依赖清理

**日期**: 2026-03-22
**版本**: 0.5.0
**类型**: Refactor + Fix

---

## 概述

本版本重点解决编译兼容性问题，收敛重复逻辑，清理遗留依赖。

---

## 变更详情

### MainViewModel 重构

| 改进项 | 说明 |
|--------|------|
| 网络流量采样 | 提炼首次采样初始化、速率计算、采样状态更新 helper |
| 导出流程 | 统一 JSON/HTML 导出中的重复状态与文件处理逻辑 |
| 状态文案 | 提炼加载完成、导入完成、导出成功、主题切换等状态 helper |

### ScottPlot 兼容处理

```csharp
// 将时间序列 X 轴转换为 ToOADate() 数值轴
var xs = _networkHistoryTimes.Select(time => time.ToOADate()).ToArray();

// 保守处理主题 API 不兼容问题
private void ApplyPlotTheme(ScottPlot.Plot plot)
{
    // 暂时缩减为空实现，降低编译风险
}
```

### HardwareService 统一骨架

- 收敛重复的 WMI 执行骨架
- 统一异常记录与 fallback 模式
- 保持现有字段映射语义不变

### 测试修复

- 修正 `ReportServiceTests` 断言类型不一致
- 统一为 `double` / `int?` 断言

---

## 依赖清理

### 移除的包

| 包名 | 原因 |
|------|------|
| `LiveChartsCore.SkiaSharpView.WPF` | 静态确认未使用，遗留依赖 |

### 移除的代码

| 文件 | 内容 |
|------|------|
| `MainWindow.xaml` | 未使用的 `xmlns:scottplot` 命名空间声明 |

---

## 取舍说明

| 决策 | 原因 |
|------|------|
| 缩减 `ApplyPlotTheme()` | 优先恢复可编译，图表主题细节延后处理 |
| 保留 ScottPlot.WPF | 作为当前实际图表实现，仅清理未使用的 LiveCharts |

---

## 验证命令

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release --no-restore
dotnet test DigYourWindows.slnx -c Release --no-build
```
