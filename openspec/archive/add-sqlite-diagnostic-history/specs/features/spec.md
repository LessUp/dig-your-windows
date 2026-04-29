## ADDED Requirements

### Requirement: Diagnostic History Persistence

系统 SHALL 在每次成功完成诊断采集后，将完整诊断结果持久化到本地 SQLite 历史库。

#### Scenario: 成功诊断后保存历史记录
- GIVEN 用户成功完成一次诊断
- WHEN `DiagnosticCollectorService` 组装出最终 `DiagnosticCollectionResult`
- THEN 系统将完整 `DiagnosticData` 保存为一条历史记录
- AND 系统同时保存用于历史列表展示的摘要字段
- AND 当前诊断结果继续正常返回给 UI

#### Scenario: 历史保存失败不影响本次结果
- GIVEN 诊断采集成功
- AND SQLite 历史保存失败
- WHEN 诊断流程结束
- THEN 当前诊断结果仍返回给 UI
- AND 系统记录错误日志
- AND 结果 warnings 中包含历史保存失败信息

### Requirement: Diagnostic History Browsing

系统 SHALL 提供最小历史浏览能力，包括最近一次诊断入口和历史记录列表。

#### Scenario: 启动时加载最近一次诊断摘要
- GIVEN 本地存在历史记录
- WHEN 应用启动并初始化主界面
- THEN 系统加载最近一次诊断的摘要信息
- AND UI 可显示最近诊断入口

#### Scenario: 浏览历史记录列表
- GIVEN 本地存在多条历史记录
- WHEN 用户打开历史记录列表
- THEN 系统按时间从新到旧返回历史摘要列表
- AND 每条记录至少包含时间、设备名、系统版本和健康分摘要

#### Scenario: 重新打开历史诊断结果
- GIVEN 用户选中一条历史记录
- WHEN 系统读取对应的完整快照
- THEN 系统将其反序列化为 `DiagnosticData`
- AND UI 复用现有结果展示流显示该记录

#### Scenario: 无历史记录
- GIVEN 本地不存在历史记录
- WHEN 应用启动或用户打开历史记录列表
- THEN 系统返回空结果
- AND UI 显示空状态
