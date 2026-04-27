# Features Specification

> **Domain**: features
> **Version**: 1.2.0
> **Status**: implemented
> **Last Updated**: 2026-04-23

## Overview

DigYourWindows 产品功能规范。记录所有已实现的核心功能及其验收标准。

---

## Requirement: Hardware Information Collection

系统 SHALL 通过 WMI 和 LibreHardwareMonitor 一键采集完整的系统硬件信息。

### Scenario: 完整硬件信息采集
- GIVEN 用户点击"Run Diagnostics"
- WHEN `DiagnosticCollectorService.CollectAsync()` 执行
- THEN 返回 CPU 型号、核心数、线程数
- AND 返回 GPU 型号和显存大小
- AND 返回内存总量、可用量、使用率
- AND 返回所有磁盘的型号、容量、接口类型
- AND 返回所有网络适配器名称和 MAC 地址
- AND 返回 USB 设备列表

### Scenario: 权限不足时降级
- GIVEN 程序未以管理员身份运行
- WHEN 采集需要管理员权限的数据（GPU 温度、SMART）
- THEN 返回 null 或默认值，不抛出异常
- AND UI 显示相关字段为"N/A"或受限状态

---

## Requirement: Real-Time Hardware Monitoring

系统 SHALL 在仪表盘上实时显示 CPU、GPU 温度/负载、网络流量，刷新间隔 ≤ 1 秒。

### Scenario: CPU 实时监控
- GIVEN 应用处于监控活跃状态
- WHEN 定时器触发（1s 间隔）
- THEN `CpuMonitorService.GetCpuData()` 返回当前温度（°C）和负载（%）
- AND ScottPlot 图表更新显示新数据点

### Scenario: GPU 实时监控
- GIVEN 系统有支持的 GPU
- WHEN 定时器触发
- THEN 返回 GPU 温度、负载百分比、显存使用量

### Scenario: 网络流量监控
- GIVEN 存在活跃网络适配器
- WHEN 定时器触发
- THEN 返回每个适配器的上传/下载速率（bytes/s）

---

## Requirement: Event Log Analysis

系统 SHALL 自动提取最近 N 天（默认 7 天）System 和 Application 日志中的错误（Level=2）和警告（Level=3）事件。

### Scenario: 事件日志提取
- GIVEN 用户触发诊断
- WHEN `EventLogService.GetErrorEvents(logName, cutoffDate)` 调用
- THEN 返回按时间降序排列的 LogEvent 列表
- AND 每条记录包含：时间戳、级别、来源、EventId、消息内容

### Scenario: 空日志处理
- GIVEN 指定日志名不存在或期间无匹配事件
- WHEN 调用事件日志查询
- THEN 返回空集合，不抛出异常

---

## Requirement: Reliability Records

系统 SHALL 读取 Windows 可靠性监视器数据，显示最近 30 天的稳定性趋势。

### Scenario: 可靠性数据获取
- GIVEN Windows 可靠性监视器服务可用
- WHEN `ReliabilityService.GetReliabilityRecords()` 调用
- THEN 返回 ReliabilityRecord 列表，每条记录含日期和稳定性指数
- AND 数据按日期排序

### Scenario: 可靠性监视器不可用
- GIVEN Windows 可靠性监视器未启用或权限不足
- WHEN 调用可靠性数据获取
- THEN 返回空集合或最后已知数据，不崩溃

---

## Requirement: System Health Scoring

系统 SHALL 基于四个维度计算 0-100 综合健康分：稳定性 40% + 性能 30% + 内存 15% + 磁盘 15%。

### Scenario: 健康分计算
- GIVEN 诊断数据收集完成
- WHEN `PerformanceService.CalculateHealthScore(data)` 调用
- THEN 返回 `PerformanceAnalysisData` 含 `SystemHealthScore` (0-100)
- AND 返回四个子分：`StabilityScore`, `PerformanceScore`, `MemoryUsageScore`, `DiskHealthScore`（均 0-100）
- AND `SystemHealthScore = round(Stability×0.4 + Performance×0.3 + MemoryUsage×0.15 + DiskHealth×0.15)`

### Scenario: 评分边界值
- GIVEN 所有传感器数据处于最差状态
- THEN `SystemHealthScore` = 0，不出现负数
- GIVEN 所有传感器数据处于最佳状态
- THEN `SystemHealthScore` = 100，不超过 100

---

## Requirement: Smart Optimization Recommendations

系统 SHALL 根据诊断数据生成优先级分类的优化建议列表。

### Scenario: 高温建议
- GIVEN CPU 温度 > 85°C 或 GPU 温度 > 90°C
- THEN 生成 High 优先级"散热警告"建议

### Scenario: 磁盘健康建议
- GIVEN 磁盘 SMART 健康状态为 "Fair" 或 "Poor"
- THEN 生成对应优先级建议（Fair=Medium, Poor=Critical）

### Scenario: 内存使用建议
- GIVEN 内存使用率 > 90%
- THEN 生成 High 优先级内存优化建议

### Scenario: 无问题时
- GIVEN 所有指标处于正常阈值内
- THEN 返回空建议列表或"系统状态良好"提示

---

## Requirement: Report Export (HTML + JSON)

系统 SHALL 将完整诊断数据导出为独立 HTML 文件或 JSON 文件，支持离线查看。

### Scenario: JSON 导出
- GIVEN 诊断数据收集完成
- WHEN 用户选择 JSON 导出
- THEN 生成合法 JSON 文件，根路径包含 `version`, `exportDate`, `toolVersion`, `data`
- AND 文件名格式：`DigYourWindows_Report_YYYY-MM-DD_HH-mm-ss.json`

### Scenario: HTML 导出
- GIVEN 诊断数据收集完成
- WHEN 用户选择 HTML 导出
- THEN 生成自包含 HTML（CSS 内嵌），无外部依赖
- AND HTML 含 system-info、hardware-info、event-logs、performance 各 section

### Scenario: 导出性能
- THEN JSON 导出 < 500ms，HTML 导出 < 1000ms

---

## Requirement: Dark/Light Theme Toggle

系统 SHALL 支持深色/浅色主题即时切换，无需重启，使用 WPF-UI 4.0 主题系统。

### Scenario: 主题切换
- GIVEN 用户点击主题切换按钮
- WHEN `ApplicationThemeManager.Apply(theme)` 调用
- THEN UI 所有组件立即切换到目标主题，无闪烁

### Scenario: 主题持久化
- GIVEN 用户切换了主题
- WHEN 应用重启
- THEN 自动恢复上次选择的主题

---

## Cancelled Features (v1.2.0 scope cut)

以下功能在 v1.2.0 收尾阶段明确取消，不会实现：

| 功能 | 原因 |
|------|------|
| CLI mode | 需求不明确，用户主要使用 GUI |
| Portable mode | FDD/SCD release 已满足需求 |
| Multi-language report export | 实现复杂，收益有限 |
| Performance benchmark comparison | 超出当前项目范围 |

## References

- [Architecture Specification](../architecture/spec.md)
- [Hardware Specification](../hardware/spec.md)
- [Export Specification](../export/spec.md)
- [Data Specification](../data/spec.md)

