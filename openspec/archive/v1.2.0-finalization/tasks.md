# Tasks: v1.2.0 Finalization

> **Change**: v1.2.0-finalization
> **Status**: ready
> **For**: GLM / Claude / any AI agent with autopilot permissions

执行这些任务时：
1. 每完成一个 task → 一次 commit（conventional format）
2. 每个 task 完成后运行 `dotnet build DigYourWindows.slnx -c Release` 确认零警告
3. 全部 tasks 完成后运行 `dotnet test DigYourWindows.slnx -c Release` 确认全通过
4. 完成后将此目录移入 `openspec/archive/`

---

## Task 1: 更新 data/spec.md 以匹配实际代码结构

**文件**: `openspec/specs/data/spec.md`

将 spec 中的 `DiagnosticData` JSON 结构更新为：

```json
{
  "hardware": {
    "computerName": "DESKTOP-ABC123",
    "osVersion": "Windows 11 Pro 23H2",
    "cpuBrand": "Intel Core i7-12700K",
    "cpuCores": 12,
    "totalMemory": 34359738368,
    "disks": [...],
    "diskSmart": [...],
    "networkAdapters": [...],
    "usbDevices": [...],
    "gpus": [...]
  },
  "events": [...],
  "reliability": [...],
  "performance": {
    "systemHealthScore": 85.5,
    "stabilityScore": 90.0,
    "performanceScore": 80.0,
    "memoryUsageScore": 85.0,
    "diskHealthScore": 88.0,
    "systemUptimeDays": 3.5,
    "criticalIssuesCount": 0,
    "warningsCount": 2,
    "recommendations": ["建议清理磁盘空间"],
    "healthGrade": "良好",
    "healthColor": "#17a2b8"
  },
  "collectedAt": "2026-04-17T10:30:00Z"
}
```

更新 `PerformanceAnalysis` 字段表（`overallScore` → `systemHealthScore`，`memoryScore` → `memoryUsageScore`，`diskScore` → `diskHealthScore`，`recommendations` 类型改为 `string[]`，新增 `systemUptimeDays`/`criticalIssuesCount`/`warningsCount`/`healthGrade`/`healthColor`）。

**Commit**: `specs(data): align data spec with actual implementation`

---

## Task 2: 补充 PerformanceService 属性测试

**文件**: `tests/DigYourWindows.Tests/PropertyTests/PerformanceServicePropertyTests.cs`（新建）

添加以下 FsCheck 属性测试：

```csharp
// Test 1: 系统健康分永远在 [0, 100] 范围内
[Property]
Property SystemHealthScore_IsAlwaysBetween0And100(
    PositiveInt errorCount, PositiveInt warningCount, 
    NonNegativeInt criticalCount, NonNegativeInt reliabilityCount)

// Test 2: 权重之和等于 1.0
[Fact]  // (not property, just sanity)
void ScoringWeights_SumToOne()

// Test 3: 空事件/空磁盘时评分不崩溃
[Property]
Property AnalyzeSystemPerformance_WithEmptyEvents_DoesNotThrow(
    NonNegativeInt cpuCores, PositiveInt memoryGB)

// Test 4: StabilityScore 随错误数增多而降低
[Property]
Property StabilityScore_DecreasesWithMoreErrors(
    PositiveInt lowerErrors, PositiveInt higherErrors)
```

**注意**: `PerformanceService` 依赖 `ISystemInfoProvider` 和 `ILogService` — 使用简单的 mock 实现（内联匿名类或测试专用实现），不引入 Moq。

**Commit**: `test(core): add FsCheck property tests for PerformanceService scoring`

---

## Task 3: 补充 DiagnosticData 序列化属性测试

**文件**: `tests/DigYourWindows.Tests/PropertyTests/DiagnosticDataPropertyTests.cs`（新建）

```csharp
// Test: 任意有效 DiagnosticData 序列化→反序列化后核心字段等价
[Property]
Property DiagnosticData_SerializationRoundTrip_PreservesAllCoreFields(
    NonEmptyString computerName, 
    NonEmptyString osVersion,
    NonNegativeInt cpuCores,
    NonNegativeInt totalMemoryMB)

// Test: CollectedAt 为 UTC 时，反序列化后仍为 UTC
[Property]
Property DiagnosticData_CollectedAt_PreservesUtcKind(
    DateTimeOffset timestamp)
```

**Commit**: `test(core): add serialization round-trip property tests for DiagnosticData`

---

## Task 4: 补充 HTML 报告生成属性测试

**文件**: `tests/DigYourWindows.Tests/PropertyTests/ReportServiceHtmlPropertyTests.cs`（新建）

```csharp
// Test: 任意有效 DiagnosticData 生成的 HTML 包含必要 sections
[Property]
Property GenerateHtmlReport_AlwaysContainsRequiredSections(
    NonEmptyString computerName, NonEmptyString osVersion)
// THEN html contains: "DigYourWindows", computerName, osVersion, "</html>"

// Test: 任意有效数据生成的 HTML 不包含未闭合标签（基础结构检查）
[Property]
Property GenerateHtmlReport_HasBalancedHtmlTags(...)
// THEN html.Contains("<html") && html.Contains("</html>")
//      html.Contains("<head") && html.Contains("</head>")
//      html.Contains("<body") && html.Contains("</body>")
```

**Commit**: `test(core): add HTML report generation property tests`

---

## Task 5: 修复 data/spec.md 中 features spec 对评分公式的描述

**文件**: `openspec/specs/features/spec.md`

在"System Health Scoring"的 Scenario 中，将字段名从 `OverallScore` 改为 `SystemHealthScore`，将 `MemoryScore` 改为 `MemoryUsageScore`，将 `DiskScore` 改为 `DiskHealthScore`。

**Commit**: `specs(features): align health scoring field names with implementation`

---

## Task 6: 更新 CHANGELOG.md 添加 v1.2.0 条目

**文件**: `CHANGELOG.md`, `docs/zh-CN/changelog.md`, `docs/en-US/changelog.md`

在 CHANGELOG.md 顶部（在 ## [1.1.0] 之前）添加：

```markdown
## [1.2.0] - 2026-04-XX

### Fixed
- Aligned data specification with actual JSON export structure
- Corrected PerformanceAnalysisData field names in documentation

### Added
- FsCheck property tests for PerformanceService scoring algorithm
- FsCheck property tests for DiagnosticData serialization round-trips
- FsCheck property tests for HTML report generation

### Changed
- Finalized feature specification with all 8 implemented features documented
- Added workflow specification (development process and AI tool collaboration)
- Cancelled: CLI mode, portable mode, multi-language report, benchmark comparison
```

**Commit**: `docs(changelog): add v1.2.0 release notes`

---

## Source Code Audit Results

Performed during Phase 7 (normalization pass). All Core Services reviewed.

### Findings

**PerformanceService — Dead Code in `GetCpuBrandScore`** (LOW severity)

The Intel and AMD "generation bonus" regex branches (lines 268-278, 302-309) are unreachable:
- Any Intel i7/i9/i5/i3 CPU is caught by the earlier `ContainsWord` checks and returns before the regex runs
- Same for AMD Ryzen 5/7/9 — caught by `ContainsWord` before AMD regex runs
- Result: 12th gen Intel i7 scores `HighTierCpuScore (10)` instead of the intended `12d`

**Decision**: Won't fix in v1.2.0 — changing scores would make existing reports inconsistent. Document as known limitation.

**All other services**: No bugs found. 
- `ReportService`: Correct XSS escaping, proper null handling, correct JSON serialization
- `PerformanceService`: All scores correctly clamped to [0, 100], weights sum to 1.0 (verified in `ScoringConfiguration`)
- `WmiSystemInfoProvider`: Correct disposal pattern with `using`
- `CalculateDiskScore`: Handles empty disk list with 50 fallback

---



```powershell
# Build
dotnet build DigYourWindows.slnx -c Release --no-restore
# Expected: Build succeeded, 0 Warning(s), 0 Error(s)

# Test
dotnet test DigYourWindows.slnx -c Release --no-restore --verbosity normal
# Expected: All tests pass (no failures, no skips except Windows-only integration tests)
```

如果所有验证通过，将此 change 归档：
```bash
mv openspec/changes/v1.2.0-finalization openspec/archive/
git add .
git commit -m "archive(finalization): complete v1.2.0 finalization tasks"
```

然后创建发布标签：
```bash
git tag v1.2.0 -m "DigYourWindows v1.2.0 — Final stable release"
git push origin v1.2.0
```
