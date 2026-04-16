# 变更日志

所有重要的项目变更都记录在此文档中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/)，版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

---

## [Unreleased]

### 计划中
- 文档国际化（英文版）
- 多语言界面支持
- 主题自定义功能

---

## [1.1.0] - 2026-04-16

### 新增
- 🌍 **文档国际化** - 完整的中英文双语文档支持
- 📚 **重构中文文档** - 优化结构，添加更多详细内容
- 🏗️ **架构文档增强** - 添加 Mermaid 架构图、性能优化说明
- ✅ **FAQ 扩展** - 添加更多常见问题解答

### 变更
- 优化 VitePress 配置，支持多语言切换
- 改进 README 结构和内容

### 文档
- 重新组织 docs 目录结构，支持国际化
- 添加英文版全部文档

---

## [1.0.0] - 2026-04-16

### 新增
- 完整的文档站重构，包括架构文档、测试指南、数据 Schema
- VitePress 本地搜索功能
- 贡献指南

### 修复
- **ScottPlot API**: 迁移至 ScottPlot 5.1 API（`LegendText`、`TitleLabel`、`Label.ForeColor`）
- **HardwareMonitorProvider**: 添加线程安全的双重检查锁定 dispose 模式
- **FileLogService**: 修复 `Dispose()` 方法的线程安全问题
- **ReportService**: 修复 `TruncateMessage()` 空字符串和 null 处理
- **MainViewModel**: 修复 `BuildReliabilityTimeline()` 在空记录时的异常

### 变更
- 移除 Converters 中冗余的 `using System;` 指令
- 统一代码风格

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2026-04-16_code-fixes-and-docs-refactor.md)

---

## [0.5.0] - 2026-03-22

### 新增
- `DiagnosticCollectorServiceTests` 测试套件
- 性能评分边界测试
- HTML 报告生成测试

### 修复
- 采集取消操作不再被普通异常处理吞掉
- `ReportServiceTests` 断言类型不一致问题

### 变更
- 重构 `DiagnosticCollectorService` 统一采集步骤骨架
- 提取 `PerformanceService` 评分辅助函数
- 拆分 `ReportService.GenerateHtmlReport()` 为多个 section helper
- 抽取 `MainViewModel` 统一的数据应用入口

### 移除
- UI 项目中未使用的 `LiveChartsCore.SkiaSharpView.WPF` 包引用
- 未使用的命名空间声明

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2026-03-22_phase2-compile-and-cleanup.md)

---

## [0.4.0] - 2026-03-13

### 修复
- 统一 `_log.Error(...)` 调用为 `_log.LogError(...)`

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2026-03-13_logservice-error-api-fix.md)

---

## [0.3.0] - 2026-03-10

### 新增
- VitePress 文档站 SEO 优化（og:* meta 标签、keywords）
- 变更日志汇总页面

### 修复
- README Docs badge 路径修复

### 变更
- Pages workflow 使用 `sparse-checkout` 替代全量 git 历史

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2026-03-10_pages-optimization.md)

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
- 统一数据契约模型

### 移除
- Rust 模块（采用全 C# 方案）

### 修复
- 异步加载的线程安全问题
- `GpuMonitorService` 生命周期管理
- 编译错误

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2025-12-14_remove-rust-and-refactor.md)

---

## [0.1.1] - 2025-02-27

### 新增
- 全面的接口抽象
- HTML 报告离线化
- 统一日志系统

### 修复
- Solution 文件补全
- 消除静默异常吞没

### 变更
- `IsCriticalError` 改为 `static readonly HashSet<uint>`
- 网络历史数据结构从 `List` 改为 `Queue`
- WMI 日期解析优化

### 移除
- 模板生成的空占位类
- 示例测试文件

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2025-02-27_project-optimization.md)

---

## [0.1.0] - 2025-02-27

### 新增
- 标准开源项目结构（`src/`、`tests/`、`docs/`）
- GitHub Actions CI/CD 配置
- Framework-dependent 和 Self-contained 双版本发布
- README.md 和 MIT LICENSE

### 变更
- 消除两层无意义目录嵌套
- Solution 文件提升到仓库根

[详细变更 →](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2025-02-27_directory-restructure.md)

---

## 版本历史

| 版本 | 日期 | 类型 | 说明 |
|------|------|------|------|
| 1.1.0 | 2026-04-16 | Minor | 文档国际化与重构 |
| 1.0.0 | 2026-04-16 | Major | 文档重构与 Bug 修复 |
| 0.5.0 | 2026-03-22 | Minor | 深度优化与测试覆盖 |
| 0.4.0 | 2026-03-13 | Patch | LogService API 修复 |
| 0.3.0 | 2026-03-10 | Minor | Pages 与 Workflow 优化 |
| 0.2.0 | 2025-12-14 | Minor | Rust 移除与 C# 重构 |
| 0.1.1 | 2025-02-27 | Patch | 项目优化 |
| 0.1.0 | 2025-02-27 | Major | 初始发布 |

---

## 已归档变更

Rust 模块相关变更已归档保留作为历史记录：

- [2025-12-04 - 报告 UI (Rust)](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2025-12-04_report-ui.md)
- [2025-12-04 - CLI/WMI/性能 (Rust)](https://github.com/LessUp/dig-your-windows/blob/main/changelog/2025-12-04_cli-wmi-perf-package.md)

---

## 升级指南

### 从 0.x 升级到 1.0

1. **API 兼容性**: 1.0 版本完全向后兼容 0.5.x
2. **数据格式**: JSON 导出格式保持不变
3. **配置文件**: 如存在，建议删除旧配置重新生成

### 从早期版本升级

建议直接下载最新版本安装包，无需保留旧配置。
