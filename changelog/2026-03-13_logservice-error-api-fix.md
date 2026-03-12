# LogService Error API 对齐

日期：2026-03-13

## 变更内容

- 将多处 `_log.Error(...)` 调用统一改为 `ILogService` 实际提供的 `_log.LogError(...)`
- 覆盖核心采集服务、事件日志服务、GPU 监控服务以及主界面 ViewModel
- 不改变异常处理语义，仅修正接口与调用方之间的命名漂移

## 背景

仓库中的 `ILogService` 只暴露 `Info`、`Warn` 与 `LogError`，但部分服务后来继续沿用旧的 `Error` 方法名，导致 CI 在编译阶段直接失败。本次调整用于恢复 .NET 主线构建通过。
