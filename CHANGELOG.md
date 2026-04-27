# Changelog

All notable changes to this project are documented in this file.

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

For Chinese version, see [docs/zh-CN/changelog.md](docs/zh-CN/changelog.md).

---

## [1.2.0] - 2026-04-27

### Fixed
- **HardwareMonitorProvider**: Resource leak on `Open()` failure - constructor now properly cleans up on exception
- README path references (`specs/` → `openspec/specs/`)

### Added
- FsCheck property tests for `PerformanceService` scoring algorithm
- FsCheck property tests for `DiagnosticData` serialization round-trips
- FsCheck property tests for HTML report generation
- Standard `CLAUDE.md` for public AI instructions
- Known limitations documentation in AGENTS.md and copilot-instructions.md

### Changed
- Normalized BMAD directories to `docs/methodology/`
- Clarified roadmap: portable mode superseded by FDD release
- Enhanced CONTRIBUTING.md with docs site reference

### Documentation
- Aligned data specification with actual JSON export structure
- Corrected `PerformanceAnalysisData` field names in feature spec

---

## [1.1.0] - 2026-04-16

### Added
- 🌍 **Complete Documentation Internationalization** - Full Chinese-English bilingual documentation
- 📚 **Refactored Chinese Documentation** - Optimized structure with significantly expanded content
- 📚 **New English Documentation** - Complete English version of all documents

### Documentation
- Restructured docs directory with language isolation (`zh-CN/`, `en-US/`)
- Updated VitePress configuration for multi-language support
- Optimized README.md structure for both languages

---

## [1.0.0] - 2026-04-16

### Added
- Complete documentation restructure with architecture, testing guide, and data schema
- VitePress local search functionality
- Contributing guidelines

### Fixed
- **ScottPlot API**: Migration to ScottPlot 5.1 API (`LegendText`, `TitleLabel`, `Label.ForeColor`)
- **HardwareMonitorProvider**: Thread-safe double-checked locking dispose pattern
- **FileLogService**: Thread-safety in `Dispose()` method
- **ReportService**: Null/empty string handling in `TruncateMessage()`
- **MainViewModel**: `InvalidOperationException` when building reliability timeline with empty records

### Changed
- Removed redundant `using System;` directives from converters
- Unified code style across UI converters

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

---

## [0.4.0] - 2026-03-13

### Fixed
- Unified `_log.Error(...)` calls to `_log.LogError(...)` across all services

---

## [0.3.0] - 2026-03-10

### Added
- VitePress SEO optimization (og:* meta tags, keywords)
- Changelog summary page in docs

### Fixed
- README docs badge URL correction

### Changed
- Pages workflow uses `sparse-checkout` instead of full git history

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
- Compilation errors

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

---

## Version History

| Version | Date | Type | Description |
|---------|------|------|-------------|
| 1.1.0 | 2026-04-16 | Minor | Documentation internationalization |
| 1.0.0 | 2026-04-16 | Major | Code fixes & docs restructure |
| 0.5.0 | 2026-03-22 | Minor | Deep optimization & test coverage |
| 0.4.0 | 2026-03-13 | Patch | LogService API fix |
| 0.3.0 | 2026-03-10 | Minor | Pages & workflow optimization |
| 0.2.0 | 2025-12-14 | Minor | Rust removal & C# refactor |
| 0.1.1 | 2025-02-27 | Patch | Project optimization |
| 0.1.0 | 2025-02-27 | Major | Initial release |
