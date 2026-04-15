# 变更日志

所有重要的项目变更都记录在此文档中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

---

## [1.0.0] - 2026-04-16

### 新增

- 完整的文档站重构，包括架构文档、测试指南、数据 Schema
- VitePress 本地搜索功能
- 贡献指南

### 修复

- **HardwareMonitorProvider**: 添加线程安全的双重检查锁定 dispose 模式，防止已释放对象访问
- **FileLogService**: 修复 `Dispose()` 方法的线程安全问题
- **ReportService**: 修复 `TruncateMessage()` 空字符串和 null 处理
- **MainViewModel**: 修复 `BuildReliabilityTimeline()` 在空记录时的异常

### 重构

- 移除 Converters 中冗余的 `using System;` 指令
- 统一代码风格

---

## [0.5.0] - 2026-03-22

### 新增

- `DiagnosticCollectorServiceTests` 测试套件
- 性能评分边界测试
- HTML 报告生成测试（maxEvents、空建议、空 GPU、HTML 编码）

### 修复

- 采集取消操作不再被普通异常处理吞掉
- `ReportServiceTests` 断言类型不一致问题

### 重构

- `DiagnosticCollectorService` 统一采集步骤骨架
- `PerformanceService` 评分阈值与权重表达拆分为辅助函数
- `ReportService.GenerateHtmlReport()` 拆分为多个 section helper
- `MainViewModel` 抽取统一的数据应用入口
- `HardwareService` 统一 WMI 执行骨架

### 移除

- UI 项目中未使用的 `LiveChartsCore.SkiaSharpView.WPF` 包引用
- `MainWindow.xaml` 中未使用的命名空间声明

---

## [0.4.0] - 2026-03-13

### 修复

- 统一 `_log.Error(...)` 调用为 `ILogService` 提供的 `_log.LogError(...)`
- 覆盖核心采集服务、事件日志服务、GPU 监控服务、主界面 ViewModel

---

## [0.3.0] - 2026-03-10

### 新增

- VitePress 文档站 SEO 优化（og:title/description/url、theme-color、keywords）
- 变更日志汇总页面

### 修复

- README.md Docs badge 路径修复

### 优化

- Pages workflow 使用 `sparse-checkout` 替代全量 git 历史

---

## [0.2.0] - 2025-12-14

### 新增

- WPF 依赖注入架构（组合根在 `App.xaml.cs`）
- JSON 导出/导入功能
- 深色/浅色主题切换
- 实时 CPU/GPU 监控
- 网络流量监控
- 可靠性趋势图表

### 变更

- 目标框架升级为 `net10.0-windows`
- 统一数据契约模型（HardwareData、GpuInfoData、LogEventData 等）

### 移除

- Rust 模块（采用全 C# 方案）

### 修复

- 异步加载的线程安全问题
- `GpuMonitorService` 生命周期管理
- 编译错误（ReportException、WmiException）

---

## [0.1.1] - 2025-02-27

### 新增

- 全面的接口抽象（IHardwareService、IReliabilityService 等）
- HTML 报告离线化（移除 Bootstrap CDN 依赖）
- 统一日志系统

### 修复

- Solution 文件补全 `DigYourWindows.Tests` 项目
- 消除静默异常吞没

### 优化

- `IsCriticalError` 改为 `static readonly HashSet<uint>`
- 网络历史数据结构从 `List.RemoveAt(0)` 改为 `Queue.Dequeue()`
- WMI 日期解析改用 `ManagementDateTimeConverter.ToDateTime()`

### 移除

- 模板生成的空占位类 `Class1.cs`
- 示例测试文件

---

## [0.1.0] - 2025-02-27

### 新增

- 标准开源项目结构（`src/`、`tests/`、`docs/`）
- GitHub Actions CI/CD 配置
- Framework-dependent 和 Self-contained 双版本发布
- README.md 项目介绍
- MIT 许可证

### 变更

- 消除两层无意义目录嵌套
- Solution 文件提升到仓库根

---

## 版本说明

| 版本类型 | 说明 |
|---------|------|
| **Major** | 不兼容的 API 变更 |
| **Minor** | 向后兼容的功能新增 |
| **Patch** | 向后兼容的问题修复 |

---

## 详细变更记录

详细的技术变更记录见 [`changelog/`](https://github.com/LessUp/dig-your-windows/tree/master/changelog) 目录：

| 日期 | 文件 | 摘要 |
|------|------|------|
| 2026-04-16 | [code-fixes-and-docs-refactor](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-04-16_code-fixes-and-docs-refactor.md) | 代码修复与文档重构 |
| 2026-03-22 | [phase2-compile-and-cleanup](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-03-22_phase2-compile-and-cleanup.md) | 编译恢复、流程收敛与依赖清理 |
| 2026-03-22 | [deep-optimization-phase1](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-03-22_deep-optimization-phase1.md) | 深度优化第一阶段收敛 |
| 2026-03-13 | [logservice-error-api-fix](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-03-13_logservice-error-api-fix.md) | LogService Error API 对齐 |
| 2026-03-10 | [pages-optimization](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-03-10-pages-optimization.md) | GitHub Pages 优化 |
| 2026-03-10 | [workflow-deep-standardization](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2026-03-10_workflow-deep-standardization.md) | Workflow 深度标准化 |
| 2025-12-14 | [remove-rust-and-refactor](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2025-12-14-remove-rust-and-refactor.md) | 移除 Rust 模块 & C# 架构重构 |
| 2025-02-27 | [project-optimization](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2025-02-27-project-optimization.md) | 项目优化与重构 |
| 2025-02-27 | [directory-restructure](https://github.com/LessUp/dig-your-windows/blob/master/changelog/2025-02-27-directory-restructure.md) | 目录结构重组 & GitHub Release 配置 |
