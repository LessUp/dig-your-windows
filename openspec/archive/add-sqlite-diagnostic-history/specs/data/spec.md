## ADDED Requirements

### Requirement: Diagnostic History Record Schema

系统 SHALL 以“完整快照 + 摘要索引字段”的方式保存历史记录。

#### Scenario: 保存完整诊断快照
- GIVEN 一次成功的诊断结果
- WHEN 系统写入历史记录
- THEN 系统保存完整 `DiagnosticData` JSON 文本
- AND 该 JSON 可在后续读取时反序列化回有效的 `DiagnosticData`

#### Scenario: 保存历史摘要字段
- GIVEN 一次成功的诊断结果
- WHEN 系统写入历史记录
- THEN 系统至少保存以下摘要字段：
- AND `id`
- AND `collectedAtUtc`
- AND `computerName`
- AND `osVersion`
- AND `cpuBrand`
- AND `totalMemoryBytes`
- AND `diskCount`
- AND `eventCount`
- AND `reliabilityRecordCount`
- AND `systemHealthScore`
- AND `healthGrade`
- AND `warningCount`
- AND `toolVersion`

#### Scenario: 历史列表按时间排序
- GIVEN 本地存在多条历史记录
- WHEN 系统查询历史摘要列表
- THEN 系统按 `collectedAtUtc` 从新到旧返回结果

#### Scenario: 最近记录查询
- GIVEN 本地至少存在一条历史记录
- WHEN 系统查询最近一次诊断
- THEN 系统返回时间最新的一条历史摘要
