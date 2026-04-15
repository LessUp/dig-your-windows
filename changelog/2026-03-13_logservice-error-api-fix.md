# LogService Error API 对齐

**日期**: 2026-03-13
**版本**: 0.4.0
**类型**: Bug Fix

---

## 概述

统一日志服务方法调用，修复 CI 编译失败问题。

---

## 问题

`ILogService` 接口只暴露 `Info`、`Warn` 与 `LogError` 方法，但部分服务继续沿用旧的 `Error` 方法名，导致编译失败。

```csharp
// 接口定义
public interface ILogService
{
    void Info(string message);
    void Warn(string message);
    void LogError(string message, Exception? exception = null);
}

// 错误调用
_log.Error("操作失败", ex);  // CS0117: 'ILogService' does not contain a definition for 'Error'
```

---

## 修复

统一所有 `_log.Error(...)` 调用为 `_log.LogError(...)`:

| 文件 | 修改 |
|------|------|
| `DiagnosticCollectorService.cs` | `_log.Error` → `_log.LogError` |
| `EventLogService.cs` | `_log.Error` → `_log.LogError` |
| `GpuMonitorService.cs` | `_log.Error` → `_log.LogError` |
| `MainViewModel.cs` | `_log.Error` → `_log.LogError` |

---

## 影响

- 不改变异常处理语义
- 仅修正接口与调用方之间的命名漂移
- CI 构建恢复正常
