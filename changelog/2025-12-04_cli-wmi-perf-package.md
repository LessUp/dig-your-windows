# [已归档] CLI/WMI/性能分析与打包改进

**日期**: 2025-12-04
**版本**: -
**类型**: Feature (Rust)
**状态**: 已归档 - Rust 模块已移除

---

## 概述

本变更为 Rust 版本的功能，自 2025-12-14 起 Rust 模块已移除，此文档仅作历史记录保留。

---

## 原始变更

### CLI 功能扩展

- `OutputFormat` 枚举：`html` / `json` / `both`
- `--output-dir` 参数：指定输出目录
- `--days` 参数：日志天数范围
- `--no-open` 参数：禁用自动打开

### WMI 鲁棒性增强

- `run_wmic_with_timeout` 辅助函数
- 30 秒超时保护
- 权限错误友好提示

### 性能分析模块

新增字段：
- `cpu_usage_percent`
- `memory_used_percent`
- `high_disk_usage_count`
- `active_network_adapters`

---

## 迁移说明

| 功能 | C# 实现 |
|------|---------|
| CLI 参数 | 暂未实现 |
| WMI 超时 | `CancellationToken` 支持 |
| 性能分析 | `PerformanceService.cs` |
