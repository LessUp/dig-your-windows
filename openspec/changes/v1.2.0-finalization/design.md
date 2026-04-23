# Design: v1.2.0 Finalization

> **Change**: v1.2.0-finalization
> **Status**: approved
> **Author**: AI-assisted (Copilot)

## Spec-Code Discrepancies Found

在审计中发现以下规范与实现之间的不一致，需要在 v1.2.0 中修复规范（调整 spec 以匹配稳定的实现）：

### 1. Data Spec 与实际 JSON 结构不一致

| Spec 描述 | 实际代码 | 处置方式 |
|-----------|---------|---------|
| `DiagnosticData.systemInfo` | `HardwareData.ComputerName/OsVersion/...` | 更新 data/spec.md 反映实际结构 |
| `DiagnosticData.hardwareData` | `DiagnosticData.Hardware` (硬件在 Hardware 中) | 同上 |
| `DiagnosticData.eventLogs` | `DiagnosticData.Events: List<LogEventData>` | 同上 |
| `DiagnosticData.reliabilityRecords` | `DiagnosticData.Reliability: List<ReliabilityRecordData>` | 同上 |
| `DiagnosticData.performanceAnalysis` | `DiagnosticData.Performance: PerformanceAnalysisData` | 同上 |

### 2. PerformanceAnalysisData 字段名不一致

| Spec 字段 | 实际字段 | 处置方式 |
|-----------|---------|---------|
| `overallScore` | `SystemHealthScore` (double) | 更新 spec 匹配代码 |
| `memoryScore` | `MemoryUsageScore` | 更新 spec |
| `diskScore` | `DiskHealthScore` | 更新 spec |
| `recommendations: [obj]` | `Recommendations: List<string>` | 更新 spec，简化类型 |

### 3. 额外字段（spec 未记录）

`PerformanceAnalysisData` 有以下 spec 未记录的字段：
- `SystemUptimeDays: double?`
- `CriticalIssuesCount: uint`
- `WarningsCount: uint`
- `HealthGrade: string` ("优秀"/"良好"/"一般"/"较差"/"需要优化")
- `HealthColor: string` (CSS hex color)

**处置**：将这些字段添加到 data/spec.md

### 4. Features Spec 评分公式

Spec 写 `OverallScore = round(Stability×0.4 + Performance×0.3 + Memory×0.15 + Disk×0.15)`，与代码实际一致（权重正确）。但字段名需要修复。

## Test Coverage Gaps

当前 `PropertyTests/` 仅有 1 个测试（ReportService 序列化往返）。需要补充：

1. **PerformanceService 评分边界** — `CalculateSystemHealthScore` 在各种极端输入下是否在 [0, 100] 之间
2. **PerformanceService 权重正确性** — 权重之和是否等于 1.0
3. **PerformanceService 空事件列表** — 无事件时评分是否合理
4. **ReportService HTML 生成** — 任意有效 DiagnosticData 生成的 HTML 是否包含必要 sections
5. **DiagnosticData 序列化往返** — 任意有效数据序列化后反序列化是否保持等价

## Implementation Plan

### 策略

优先修复 spec（低风险），然后补充测试（中等工作量），最后检查代码 bug（按需）。

### 调整原则

- **不修改公共 API**：不重命名现有字段（会破坏已导出的 JSON 文件的兼容性）
- **更新规范以匹配代码**：spec 是描述性文档，应反映现实
- **测试驱动质量**：新增的 property tests 作为质量门禁

## Files to Change

| 文件 | 变更类型 |
|------|---------|
| `openspec/specs/data/spec.md` | 更新 JSON 结构和字段名 |
| `tests/.../PropertyTests/` | 新增 4-5 个属性测试 |
| `CHANGELOG.md` | 添加 v1.2.0 条目 |
| `docs/*/changelog.md` | 同步更新 |
