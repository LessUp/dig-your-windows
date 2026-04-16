# 变更日志

所有重要的项目变更都记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/)，版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

---

## [Unreleased]

### 计划中
- 🌍 多语言界面支持
- 📊 导出格式扩展（CSV、PDF）
- 🔌 插件系统架构

---

## [1.1.0] - 2026-04-16

### 新增
- 🌍 **完整文档国际化** - 中英文双语文档全面上线
- 📚 **中文文档重构** - 优化结构，内容大幅扩充
- 📚 **英文文档新增** - 完整的英文版文档
- 🔧 **VitePress 多语言配置** - 支持语言切换

### 变更
- 优化 README.md 结构和内容
- 改进 VitePress 配置，添加国际化支持

### 文档
- 重构 docs 目录结构，采用语言隔离
- 新增英文版全部文档（快速开始、架构、测试指南、FAQ、数据 Schema、变更日志）

---

## [1.0.0] - 2026-04-16

### 新增
- 📚 完整文档站重构
  - 架构文档（Architecture.md）- 详细的技术架构说明
  - 测试指南（Testing.md）- 测试策略和最佳实践
  - 数据 Schema（data-schema.md）- JSON 数据格式参考
  - 贡献指南（Contributing.md）- 开发贡献指南
  - FAQ（faq.md）- 常见问题解答
- 🔍 VitePress 本地搜索功能
- 📖 VitePress SEO 优化（og 标签、keywords）

### 修复
- **ScottPlot API 兼容性**: 迁移到 ScottPlot 5.1 API
  - `Scatter.Label` → `LegendText`
  - `LabelStyle.Style.ForeColor` → `Label.ForeColor`
  - `Plot.Title.LabelStyle` → `TitleLabel`
- **HardwareMonitorProvider**: 添加线程安全的双重检查锁定 dispose 模式
  - 添加 `ObjectDisposedException` 保护
  - 修复 dispose 后仍可访问的问题
- **FileLogService**: 修复 `Dispose()` 方法线程安全问题
  - `_writer.Dispose()` 移至 lock 块内
- **ReportService**: 修复 `TruncateMessage()` 空字符串和 null 处理
- **MainViewModel**: 修复 `BuildReliabilityTimeline()` 空记录异常

### 变更
- 移除 Converters 中冗余的 `using System;` 指令
- 统一 UI Converters 代码风格

---

## [0.5.0] - 2026-03-22

### 新增
- 🧪 `DiagnosticCollectorServiceTests` 测试套件（12 个测试用例）
- 🧪 性能评分边界测试（健康等级边界值）
- 🧪 HTML 报告生成测试（标题、样式、内容验证）
- 📝 详细变更日志文档（Phase 2）

### 修复
- 采集取消操作不再被普通异常处理吞掉（CancellationToken 正确传播）
- `ReportServiceTests` 断言类型不一致（`Assert.IsType` → `Assert.IsAssignableFrom`）

### 变更
- 重构 `DiagnosticCollectorService`，统一采集步骤骨架设计
- 提取 `PerformanceService` 评分辅助函数（单一职责）
- 拆分 `ReportService.GenerateHtmlReport()` 为 section helper
- 抽取 `MainViewModel` 统一的数据应用入口 `ApplyDiagnosticData()`

### 移除
- UI 项目中未使用的 `LiveChartsCore.SkiaSharpView.WPF` 包引用
- `MainWindow.xaml` 未使用的命名空间声明
- `DiagnosticCollectorService` 中未使用的字段 `_reliabilityService`

---

## [0.4.0] - 2026-03-13

### 修复
- 统一所有服务的 `_log.Error(...)` 调用为 `_log.LogError(...)`
  - 影响范围：`ReliabilityService.cs`, `PerformanceService.cs`
  - 确保与 `ILogService` 接口定义一致

---

## [0.3.0] - 2026-03-10

### 新增
- 🔍 VitePress 文档站 SEO 优化
  - `og:*` meta 标签（Open Graph）
  - 关键词 meta 标签
- 📋 变更日志汇总页面（docs/changelog.md）

### 修复
- README Docs badge URL 路径修正

### 变更
- Pages workflow 使用 `sparse-checkout` 替代全量 git 历史
  - 显著减少 CI 检出时间

---

## [0.2.0] - 2025-12-14

### 新增
- 🏗️ WPF 依赖注入架构（组合根在 `App.xaml.cs`）
- 📤 JSON 导出/导入功能
- 🌓 深色/浅色主题切换
- 📊 实时 CPU/GPU 监控（LibreHardwareMonitor）
- 📊 网络流量实时监控
- 📈 可靠性趋势图表（ScottPlot）

### 变更
- Target Framework 升级到 `net10.0-windows`
- 统一数据契约模型（命名规范化）

### 移除
- Rust 模块（完全迁移到 C# 方案）
- 相关的 Cargo.toml、.rs 源文件、rust-toolchain.toml

### 修复
- 异步加载的线程安全问题
- `GpuMonitorService` 生命周期管理（dispose 问题）
- 编译错误（ReportException、WmiException 构造函数）

---

## [0.1.1] - 2025-02-27

### 新增
- 🔌 全面的接口抽象（所有核心服务）
- 📄 HTML 报告离线化（嵌入 CSS 样式）
- 📝 统一日志系统（FileLogService）

### 修复
- Solution 文件补全（添加测试项目）
- 消除静默异常吞没（增加日志记录）

### 变更
- `IsCriticalError` 改为 `static readonly HashSet<uint>`
- 网络历史数据结构从 `List` 改为 `Queue`（性能优化）
- WMI 日期解析使用 `ManagementDateTimeConverter`

### 移除
- 模板生成的空占位类 `Class1.cs`
- 示例测试文件

---

## [0.1.0] - 2025-02-27

### 新增
- 📁 标准开源项目结构（`src/`、`tests/`、`docs/`）
- 🔄 GitHub Actions CI/CD 配置
- 📦 Framework-dependent 和 Self-contained 双版本发布
- 📄 README.md 和 MIT LICENSE

### 变更
- 消除两层无意义目录嵌套
- Solution 文件提升到仓库根目录

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

## 类别说明

| Emoji | 分类 | 说明 |
|-------|------|------|
| ✨ | `Added` | 新增功能 |
| 🔧 | `Changed` | 功能变更 |
| 🗑️ | `Deprecated` | 即将移除的功能 |
| 🚫 | `Removed` | 已移除的功能 |
| 🐛 | `Fixed` | Bug 修复 |
| 🔒 | `Security` | 安全相关 |

---

## 已归档变更

Rust 模块相关历史变更：
- [2025-12-04 - 报告 UI (Rust)](2025-12-04_report-ui.md)
- [2025-12-04 - CLI/WMI/性能 (Rust)](2025-12-04_cli-wmi-perf-package.md)
