# 深度优化第一阶段收敛

**日期**: 2026-03-22
**版本**: 0.5.0
**类型**: Refactor + Test

---

## 概述

本版本重点解决采集编排、评分规则、报告生成和主界面流程重复等质量问题。

---

## 变更详情

### DiagnosticCollectorService

**问题**: 采集取消会被普通异常处理吞掉，UI 无法正确感知取消。

**修复**:
```csharp
private async Task<T> ExecuteStepAsync<T>(...)
{
    try
    {
        return await Task.Run(() => operation(cancellationToken), cancellationToken);
    }
    catch (OperationCanceledException)
    {
        throw;  // 不再被降级吞掉
    }
    catch (Exception ex)
    {
        // 其他异常正常处理
    }
}
```

**新增测试**:
- 取消传播测试
- warning + fallback 测试
- 进度顺序测试
- 结果装配测试

### PerformanceService

| 改进项 | 说明 |
|--------|------|
| 评分阈值 | 拆分为 `ScoringConfiguration` 常量类 |
| 评分辅助函数 | 提取 `CalculateMemoryScore`、`CalculateDiskScore`、`CalculateStabilityScore` 等 |
| 边界测试 | 补充内存/磁盘/可靠性边界测试 |

### ReportService

**重构**: 将 `GenerateHtmlReport()` 拆分为多个 section helper:

```csharp
private static void AppendDocumentStart(StringBuilder sb, DateTime collectedAt);
private static void AppendOverviewSection(StringBuilder sb, HardwareData hardware);
private static void AppendPerformanceSection(StringBuilder sb, PerformanceAnalysisData? performance);
private static void AppendGpuSection(StringBuilder sb, List<GpuInfoData> gpus);
private static void AppendEventsSection(StringBuilder sb, List<LogEventData> events, int daysBackForEvents, int maxEvents);
private static void AppendDocumentEnd(StringBuilder sb);
```

**新增测试**:
- `maxEvents` 边界测试
- 空建议测试
- 空 GPU 测试
- 未知 uptime 测试
- HTML 编码测试

### MainViewModel

**改进**:
- 抽取统一的数据应用入口 `ApplyDiagnosticData()`
- 复用导入/加载后的 UI 回填流程
- 增加"无有效数据时阻止导出"的保护

---

## 测试覆盖

| 测试类 | 覆盖内容 |
|--------|----------|
| `DiagnosticCollectorServiceTests` | 取消、警告、进度、结果装配 |
| `PerformanceServiceTests` | 评分边界、建议生成 |
| `ReportServiceTests` | 报告生成、HTML 编码 |
