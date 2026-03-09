# 变更日志

项目各阶段的重要变更记录。完整历史见 [`changelog/`](https://github.com/LessUp/dig-your-windows/tree/master/changelog) 目录。

## 2025-12-14 — 移除 Rust 模块 & C# 架构重构

- 采用全 C# 方案（WPF + C# 采集/分析），不再维护 Rust 模块
- WPF 引入 DI（组合根在 `App.xaml.cs`），`MainWindow` / `MainViewModel` 改为构造函数注入
- 统一数据契约（`HardwareData`、`GpuInfoData`、`LogEventData`、`ReliabilityRecordData`、`PerformanceAnalysisData`）
- 对齐目标框架为 `net10.0-windows`
- 修复异步加载的线程安全问题
- 新增 JSON 导出/导入功能

## 2025-02-27 — 项目优化与重构

### 架构改进
- 全面接口抽象：为所有核心服务提取接口（`IHardwareService`、`IReliabilityService`、`IEventLogService` 等）
- DI 注册全部改为接口到实现的注册方式
- `MainViewModel` 依赖接口而非具体类

### 性能优化
- `IsCriticalError` 关键错误 EventId 集合改为 `static readonly HashSet<uint>`
- 网络历史数据结构 `List.RemoveAt(0)` (O(n)) 改为 `Queue.Dequeue()` (O(1))
- WMI 日期解析改用内置 `ManagementDateTimeConverter.ToDateTime()`

### 其他
- HTML 报告离线化：移除 Bootstrap CDN 依赖，改为内嵌自包含 CSS
- 统一日志系统，消除静默异常吞没

## 2025-02-27 — 目录结构重组 & Release 配置

- 消除两层无意义嵌套，采用标准开源项目结构（`src/`、`tests/`、`docs/`、`installer/`、`scripts/`）
- 新增 `release.yml` — 推送 `v*` tag 自动构建并发布 Release（Framework-dependent + Self-contained）
- CI 更新为使用仓库根的 slnx 文件
