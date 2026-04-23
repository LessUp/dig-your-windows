# DigYourWindows

[![CI](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)
[![Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Release](https://img.shields.io/github/v/release/LessUp/dig-your-windows)](https://github.com/LessUp/dig-your-windows/releases)
[![Downloads](https://img.shields.io/github/downloads/LessUp/dig-your-windows/total)](https://github.com/LessUp/dig-your-windows/releases)
[![Tests](https://img.shields.io/badge/Tests-passing-green)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)

English | [简体中文](README.zh-CN.md)

> 📖 **Documentation**: [https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

**Windows Deep Diagnostics Tool** — One-click hardware information collection, event log analysis, reliability records, system health scoring, and optimization recommendations.

## 📸 Screenshots

![DigYourWindows Overview](https://raw.githubusercontent.com/LessUp/dig-your-windows/master/docs/public/screenshot-overview.png)

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🔍 **Hardware Detection** | Collect CPU, GPU, RAM, Disk (with SMART), Network adapters, USB devices |
| 📊 **Real-Time Monitoring** | CPU temp/load/frequency, GPU temp/load/VRAM, network traffic |
| 📋 **Event Log Analysis** | Auto-extract System/Application errors and warnings |
| 📈 **Reliability Records** | Windows Reliability Monitor data with trend visualization |
| ✅ **Health Scoring** | Comprehensive stability, performance, memory, disk assessment |
| 💡 **Smart Recommendations** | Rule-based intelligent optimization suggestions |
| 📄 **Report Export** | HTML and JSON formats with offline viewing |
| 🎨 **Theme Support** | Dark/Light theme instant toggle |

## 🚀 Quick Start

### System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| OS | Windows 10 (Build 19041+) | Windows 11 |
| RAM | 4 GB | 8 GB+ |
| Disk | 200 MB free space | 500 MB+ |
| Display | 1280x720 | 1920x1080+ |
| Privileges | Standard user | Administrator (for full hardware access) |

### Prerequisites

- Windows 10/11 (Build 19041+)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (for building)

### Installation

#### Option 1: Download Release (Recommended)

Download the latest installer from [Releases](https://github.com/LessUp/dig-your-windows/releases):

| Version | Size | Requirements |
|---------|------|--------------|
| `DigYourWindows_Setup.exe` | ~5MB | Downloads .NET if needed |
| FDD (Framework-Dependent) | ~60MB | Requires .NET 10 Runtime |
| SCD (Self-Contained) | ~180MB | No dependencies |

#### Option 2: Build from Source

```powershell
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet restore
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ **Note**: Some features (GPU monitoring, SMART data) require administrator privileges.

### First Run

1. Launch `DigYourWindows.UI.exe` (or run via `dotnet run`)
2. Click "Run Diagnostics" to collect system information
3. View real-time hardware monitoring on the dashboard
4. Export reports via JSON or HTML format

## 🏗️ Tech Stack

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| Runtime | .NET + WPF | 10.0 | Desktop application framework |
| UI Library | WPF-UI | 4.0 | Fluent Design components |
| MVVM | CommunityToolkit.Mvvm | 8.4 | Data binding & commands |
| Charts | ScottPlot | 5.1 | Performance visualization |
| Hardware | LibreHardwareMonitor | 0.9 | CPU/GPU temp, load, frequency |
| Testing | xUnit + FsCheck | 2.9 / 2.16 | Unit & property-based tests |

## 📁 Project Structure

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # Core business logic (services, models, exceptions)
│   └── DigYourWindows.UI/       # WPF user interface (MVVM, converters, XAML)
├── tests/
│   └── DigYourWindows.Tests/    # Unit, Property (FsCheck), Integration tests
├── openspec/                    # OpenSpec specification documents (source of truth)
│   ├── specs/                   # Domain specifications (architecture, hardware, export…)
│   └── changes/                 # Active change proposals
├── docs/                        # VitePress documentation site
│   ├── zh-CN/                   # Chinese documentation (primary)
│   ├── en-US/                   # English documentation
│   ├── public/                  # Static assets (images, icons)
│   └── .vitepress/              # VitePress configuration & theme
├── installer/                   # Inno Setup scripts
└── scripts/                     # Build & release scripts
```

## 🧪 Testing

```powershell
# Run all tests
dotnet test DigYourWindows.slnx

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Filter specific tests
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

## 🗺️ Roadmap

- [x] Hardware information collection (CPU, GPU, RAM, Disk, Network)
- [x] Real-time monitoring dashboard with charts
- [x] Event log analysis and reliability records
- [x] System health scoring algorithm
- [x] HTML/JSON report export
- [x] Dark/Light theme support
- [ ] Command-line interface (CLI) mode
- [ ] Portable mode (no installation required)
- [ ] Multi-language report export
- [ ] Performance benchmark comparison

## 📚 Documentation

- [📖 Getting Started Guide](https://lessup.github.io/dig-your-windows/en-US/guide/getting-started)
- [🏗️ Architecture Documentation](https://lessup.github.io/dig-your-windows/en-US/guide/architecture)
- [🧪 Testing Guide](https://lessup.github.io/dig-your-windows/en-US/guide/testing)
- [📊 Data Schema Reference](https://lessup.github.io/dig-your-windows/en-US/reference/data-schema)
- [❓ FAQ](https://lessup.github.io/dig-your-windows/en-US/guide/faq)
- [📝 Changelog](CHANGELOG.md)

> 💡 **Offline Access**: Documentation is also available in the `/docs` directory of this repository.

Documentation is available in **English** and **简体中文**.

## 🤝 Contributing

We welcome contributions! This project follows **Spec-Driven Development (SDD)** workflow.

### Quick Start

1. Read [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines
2. Review existing [specifications](specs/) before starting work
3. Create or update specs for new features
4. Submit a Pull Request following conventional commits

### Contributing Process

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. **Review/create specs** in `/specs/` directory
4. Implement according to specs
5. Write tests based on acceptance criteria
6. Commit changes (`git commit -m 'feat: add amazing feature'`)
7. Push to branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for the complete guide including SDD workflow instructions.

## 📦 Release Process

Push a `v*` tag to trigger automatic release:

```powershell
git tag v1.1.0
git push origin v1.1.0
```

GitHub Actions will automatically build and publish the release with bilingual release notes.

## 🔒 Security

This tool:
- ✅ Runs entirely offline
- ✅ Does not upload any data to external servers
- ✅ Uses local file system for all operations
- ✅ Is open source and auditable

## ⭐ Star History

[![Star History Chart](https://api.star-history.com/svg?repos=LessUp/dig-your-windows&type=Date)](https://star-history.com/#LessUp/dig-your-windows&Date)

## 📄 License

[MIT License](LICENSE) - Copyright © 2025-2026 LessUp

---

<p align="center">
  Made with ❤️ by <a href="https://github.com/LessUp">LessUp</a>
</p>
