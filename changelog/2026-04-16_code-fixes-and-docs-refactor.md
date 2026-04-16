# 代码修复与文档重构

**日期**: 2026-04-16
**版本**: 1.0.0
**类型**: Bug Fix + Documentation

---

## 概述

本版本修复了多个潜在的运行时错误，解决 ScottPlot API 兼容性问题，并完成项目文档的全面重构。

---

## 代码修复

### ScottPlot API 兼容性

**问题**: ScottPlot 5.1 API 变更导致编译失败：
- `Scatter.Label` 已过时，应使用 `LegendText`
- `LabelStyle.Style.ForeColor` 不存在，应使用 `Label.ForeColor`
- `Plot.Title.LabelStyle` 不存在，应使用 `TitleLabel`

**修复**:
```csharp
// Scatter 标签
scatter.LegendText = "下载";  // 原 scatter.Label

// 轴标签颜色
axis.Label.ForeColor = textColor;  // 原 axis.Label.Style.ForeColor

// 标题标签颜色
plot.Axes.Title.Label.ForeColor = textColor;  // 原 plot.Title.LabelStyle.ForeColor
```

### HardwareMonitorProvider

**问题**: dispose 后仍可访问 `Computer` 属性，可能导致空引用异常；dispose 方法非线程安全。

**修复**:
```csharp
// 添加线程安全的双重检查锁定 dispose 模式
public void Dispose()
{
    if (_disposed) return;

    lock (_lock)
    {
        if (_disposed) return;
        _disposed = true;
        _computer?.Close();
        _computer = null;
    }
}

// 添加 ObjectDisposedException 保护
public Computer Computer
{
    get
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _computer!;
    }
}
```

### FileLogService

**问题**: `Dispose()` 方法在锁外访问 `_writer`，存在线程安全风险。

**修复**: 将 `_writer.Dispose()` 移入 lock 块内，并添加空值条件检查。

### ReportService

**问题**: `TruncateMessage()` 未处理 null 或空字符串，可能导致异常。

**修复**: 添加 null/empty 检查。

### MainViewModel

**问题**: 当 `SelectedDaysBack <= 0` 且记录为空时，`records.Min()` 抛出 `InvalidOperationException`。

**修复**: 添加空列表检查。

### Converters

**变更**: 移除冗余的 `using System;` 指令，统一代码风格。

---

## 文档重构

### README

- 重新组织功能列表为表格格式
- 添加技术栈详细版本信息
- 完善快速开始指南
- 添加贡献指南章节

### 架构文档

- 详细描述六大核心设计
- 添加代码示例展示关键实现
- 补充依赖注入配置说明
- 添加异常处理策略说明

### 测试指南

- 完善测试命令和选项说明
- 添加 Stub/Mock 使用示例
- 添加最佳实践章节

### 数据 Schema

- 补充所有数据模型的完整字段说明
- 添加 JSON 示例
- 详细说明健康评分计算方法
- 添加枚举值对照表

### VitePress 配置

- 优化导航结构
- 添加本地搜索配置
- 完善 SEO 元数据

---

## 影响范围

| 组件 | 影响 |
|------|------|
| Core/Services | 线程安全修复 |
| UI/ViewModels | 空值处理修复、ScottPlot API 适配 |
| UI/Converters | 代码清理 |
| docs/ | 全面重构 |

---

## 验证命令

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release --no-restore
dotnet test DigYourWindows.slnx -c Release --no-build
```
