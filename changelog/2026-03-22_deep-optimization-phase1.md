# 深度优化第一阶段收敛

日期：2026-03-22

## 变更内容

- 重构 `DiagnosticCollectorService` 的阶段执行流程，统一采集步骤骨架，并确保 `OperationCanceledException` 不再被降级吞掉
- 新增 `DiagnosticCollectorServiceTests`，覆盖取消传播、warning + fallback、进度顺序与结果装配
- 收敛 `PerformanceService` 的评分阈值与权重表达，拆分评分辅助函数，并补充内存/磁盘/可靠性边界测试
- 将 `ReportService.GenerateHtmlReport()` 拆分为文档头、概览、性能、GPU、事件表等 section helper，补充 `maxEvents`、空建议、空 GPU、未知 uptime 与 HTML 编码测试
- 在 `MainViewModel` 中抽取统一的数据应用入口，复用导入/加载后的 UI 回填流程，并为 JSON/HTML 导出增加“无有效数据时阻止导出”的保护

## 背景

项目当前的主要质量风险集中在采集编排、评分规则、报告生成和主界面流程重复上。其中最直接的问题是采集取消会被普通异常处理吞掉，导致 UI 侧无法正确感知取消；同时 `MainViewModel` 和 `ReportService` 存在明显的重复与长方法维护成本。

## 验证说明

- 已补充并更新对应单元测试文件，覆盖 Collector / Performance / Report 三块核心逻辑
- 尝试执行 `dotnet restore/build/test` 进行本地验证，但当前环境缺少 `dotnet` 命令，无法在此会话内完成构建与测试运行
- 建议在具备 .NET SDK 的环境中继续执行：`dotnet restore DigYourWindows.slnx && dotnet build DigYourWindows.slnx -c Release --no-restore && dotnet test DigYourWindows.slnx -c Release --no-build`
