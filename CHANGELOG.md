# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [1.0.0] - 2026-04-16

### Added
- Complete documentation restructure with architecture, testing guide, and data schema
- VitePress local search functionality
- Contributing guidelines

### Fixed
- **ScottPlot API**: Migrate to ScottPlot 5.1 API (`LegendText`, `TitleLabel`, `Label.ForeColor`)
- **HardwareMonitorProvider**: Thread-safe double-checked locking dispose pattern
- **FileLogService**: Thread-safety in `Dispose()` method
- **ReportService**: Null/empty string handling in `TruncateMessage()`
- **MainViewModel**: `InvalidOperationException` when building reliability timeline with empty records

### Changed
- Removed redundant `using System;` directives from converters
- Unified code style across UI converters

[Details →](changelog/2026-04-16_code-fixes-and-docs-refactor.md)

---

## [0.5.0] - 2026-03-22

### Added
- `DiagnosticCollectorServiceTests` test suite
- Performance scoring boundary tests
- HTML report generation tests

### Fixed
- Cancellation token propagation in diagnostic collection
- Assertion type mismatch in `ReportServiceTests`

### Changed
- Refactored `DiagnosticCollectorService` with unified step execution skeleton
- Extracted scoring helper functions in `PerformanceService`
- Split `ReportService.GenerateHtmlReport()` into section helpers
- Unified data application flow in `MainViewModel`

### Removed
- Unused `LiveChartsCore.SkiaSharpView.WPF` package reference
- Unused namespace declarations in `MainWindow.xaml`

[Details →](changelog/2026-03-22_phase2-compile-and-cleanup.md)

---

## [0.4.0] - 2026-03-13

### Fixed
- Unified `_log.Error(...)` calls to `_log.LogError(...)` across all services

[Details →](changelog/2026-03-13_logservice-error-api-fix.md)

---

## [0.3.0] - 2026-03-10

### Added
- VitePress SEO optimization (og:* meta tags, keywords)
- Changelog summary page in docs

### Fixed
- README docs badge URL correction

### Changed
- Pages workflow uses `sparse-checkout` instead of full git history

[Details →](changelog/2026-03-10_pages-optimization.md)

---

## [0.2.0] - 2025-12-14

### Added
- Dependency injection architecture in WPF
- JSON export/import functionality
- Dark/light theme toggle
- Real-time CPU/GPU monitoring
- Network traffic monitoring
- Reliability trend charts

### Changed
- Target framework upgraded to `net10.0-windows`
- Unified data contract models

### Removed
- Rust module (fully migrated to C#)

### Fixed
- Thread safety in async loading
- `GpuMonitorService` lifecycle management
- Compilation errors (ReportException, WmiException)

[Details →](changelog/2025-12-14_remove-rust-and-refactor.md)

---

## [0.1.1] - 2025-02-27

### Added
- Full interface abstraction for all core services
- Offline HTML reports (embedded CSS)

### Fixed
- Solution file missing test project
- Silent exception swallowing

### Changed
- `IsCriticalError` uses `static readonly HashSet<uint>`
- Network history uses `Queue` instead of `List`
- WMI date parsing uses `ManagementDateTimeConverter`

### Removed
- Template placeholder class `Class1.cs`
- Sample test files

[Details →](changelog/2025-02-27_project-optimization.md)

---

## [0.1.0] - 2025-02-27

### Added
- Standard open-source project structure (`src/`, `tests/`, `docs/`)
- GitHub Actions CI/CD configuration
- Framework-dependent and self-contained release builds
- README.md and MIT LICENSE

### Changed
- Eliminated two-level directory nesting
- Solution file moved to repository root

[Details →](changelog/2025-02-27_directory-restructure.md)

---

## Version History

| Version | Date | Type | Description |
|---------|------|------|-------------|
| 1.0.0 | 2026-04-16 | Patch | Code fixes & docs restructure |
| 0.5.0 | 2026-03-22 | Minor | Deep optimization & test coverage |
| 0.4.0 | 2026-03-13 | Patch | LogService API fix |
| 0.3.0 | 2026-03-10 | Minor | Pages & workflow optimization |
| 0.2.0 | 2025-12-14 | Minor | Rust removal & C# refactor |
| 0.1.1 | 2025-02-27 | Patch | Project optimization |
| 0.1.0 | 2025-02-27 | Major | Initial release |

---

## Archived Changes

Changes related to the removed Rust module are archived for historical reference:

- [2025-12-04 - Report UI (Rust)](changelog/2025-12-04_report-ui.md)
- [2025-12-04 - CLI/WMI/Perf (Rust)](changelog/2025-12-04_cli-wmi-perf-package.md)
