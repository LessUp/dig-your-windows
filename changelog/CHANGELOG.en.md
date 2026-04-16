# Changelog

All notable changes to this project are documented in this file.

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [Unreleased]

### Planned
- 🌍 Multi-language UI support
- 📊 Additional export formats (CSV, PDF)
- 🔌 Plugin system architecture

---

## [1.1.0] - 2026-04-16

### Added
- 🌍 **Complete Documentation Internationalization** - Full Chinese-English bilingual documentation
- 📚 **Refactored Chinese Documentation** - Optimized structure with significantly expanded content
- 📚 **New English Documentation** - Complete English version of all documents
- 🔧 **VitePress Multi-language Configuration** - Language switching support

### Changed
- Optimized README.md structure and content
- Improved VitePress configuration with internationalization support

### Documentation
- Restructured docs directory with language isolation
- Added complete English documentation (Getting Started, Architecture, Testing Guide, FAQ, Data Schema, Changelog)

---

## [1.0.0] - 2026-04-16

### Added
- 📚 Complete documentation site restructure
  - Architecture documentation with detailed technical explanations
  - Testing guide with strategy and best practices
  - Data Schema reference for JSON formats
  - Contributing guide for developers
  - FAQ with common questions and troubleshooting
- 🔍 VitePress local search functionality
- 📖 VitePress SEO optimization (og tags, keywords)

### Fixed
- **ScottPlot API Compatibility**: Migration to ScottPlot 5.1 API
  - `Scatter.Label` → `LegendText`
  - `LabelStyle.Style.ForeColor` → `Label.ForeColor`
  - `Plot.Title.LabelStyle` → `TitleLabel`
- **HardwareMonitorProvider**: Thread-safe double-checked locking dispose pattern
  - Added `ObjectDisposedException` protection
  - Fixed post-dispose access issue
- **FileLogService**: Thread-safety in `Dispose()` method
  - Moved `_writer.Dispose()` inside lock block
- **ReportService**: Null/empty string handling in `TruncateMessage()`
- **MainViewModel**: Fixed `BuildReliabilityTimeline()` null records exception

### Changed
- Removed redundant `using System;` directives from converters
- Unified UI Converters code style

---

## [0.5.0] - 2026-03-22

### Added
- 🧪 `DiagnosticCollectorServiceTests` test suite (12 test cases)
- 🧪 Performance scoring boundary tests
- 🧪 HTML report generation tests (title, style, content validation)
- 📝 Detailed changelog documentation (Phase 2)

### Fixed
- Cancellation token propagation in diagnostic collection
- `ReportServiceTests` assertion type mismatch

### Changed
- Refactored `DiagnosticCollectorService` with unified step execution pattern
- Extracted scoring helper functions in `PerformanceService`
- Split `ReportService.GenerateHtmlReport()` into section helpers
- Extracted `MainViewModel` unified data application entry

### Removed
- Unused `LiveChartsCore.SkiaSharpView.WPF` package reference
- Unused namespace declarations in `MainWindow.xaml`

---

## [0.4.0] - 2026-03-13

### Fixed
- Unified `_log.Error(...)` calls to `_log.LogError(...)` across all services

---

## [0.3.0] - 2026-03-10

### Added
- 🔍 VitePress documentation SEO optimization
  - `og:*` meta tags (Open Graph)
  - Keywords meta tags
- 📋 Changelog summary page

### Fixed
- README Docs badge URL correction

### Changed
- Pages workflow uses `sparse-checkout` instead of full git history

---

## [0.2.0] - 2025-12-14

### Added
- 🏗️ WPF dependency injection architecture
- 📤 JSON export/import functionality
- 🌓 Dark/light theme toggle
- 📊 Real-time CPU/GPU monitoring
- 📊 Real-time network traffic monitoring
- 📈 Reliability trend charts

### Changed
- Target Framework upgraded to `net10.0-windows`

### Removed
- Rust module (fully migrated to C#)

### Fixed
- Thread safety in async loading
- `GpuMonitorService` lifecycle management
- Compilation errors

---

## Version History

| Version | Date | Type | Description |
|---------|------|------|-------------|
| 1.1.0 | 2026-04-16 | Minor | Documentation internationalization |
| 1.0.0 | 2026-04-16 | Major | Documentation restructure & bug fixes |
| 0.5.0 | 2026-03-22 | Minor | Deep optimization & test coverage |
| 0.4.0 | 2026-03-13 | Patch | LogService API fix |
| 0.3.0 | 2026-03-10 | Minor | Pages & workflow optimization |
| 0.2.0 | 2025-12-14 | Minor | Rust removal & C# refactor |

---

## Category Legend

| Emoji | Category | Description |
|-------|----------|-------------|
| ✨ | `Added` | New features |
| 🔧 | `Changed` | Changes to existing functionality |
| 🗑️ | `Deprecated` | Soon-to-be removed features |
| 🚫 | `Removed` | Now removed features |
| 🐛 | `Fixed` | Bug fixes |
| 🔒 | `Security` | Security-related changes |
