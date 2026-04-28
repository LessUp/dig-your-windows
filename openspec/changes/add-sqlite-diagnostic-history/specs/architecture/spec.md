## ADDED Requirements

### Requirement: Diagnostic History Subsystem

系统 SHALL 引入独立的历史存储子系统来管理本地历史持久化，而不是将历史逻辑混入现有采集或导出服务。

#### Scenario: 历史存储服务边界清晰
- GIVEN 应用配置依赖注入
- WHEN 历史能力被注册
- THEN 系统通过独立接口 `IHistoryStoreService` 暴露历史初始化、保存和读取能力
- AND 采集逻辑继续由 `DiagnosticCollectorService` 负责
- AND 导出逻辑继续由 `ReportService` 负责

#### Scenario: 诊断编排服务调用历史存储
- GIVEN 一次诊断采集流程成功完成
- WHEN `DiagnosticCollectorService` 返回结果前执行后处理
- THEN 系统调用历史存储服务保存结果
- AND 历史存储失败不会导致采集流程整体失败

#### Scenario: 应用启动加载历史摘要
- GIVEN 应用启动
- WHEN ViewModel 初始化
- THEN 系统通过历史存储服务加载最近记录摘要和历史列表
- AND 历史读失败不会阻止主界面启动
