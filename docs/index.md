---
layout: home

hero:
  name: DigYourWindows
  text: Windows Deep Diagnostics Tool
  tagline: One-click hardware info collection, event log analysis, reliability records, system health scoring & optimization recommendations
  image:
    src: /hero-image.png
    alt: DigYourWindows Interface Preview
  actions:
    - theme: brand
      text: 🚀 Get Started
      link: /en-US/guide/getting-started
    - theme: alt
      text: 📖 Documentation
      link: /en-US/guide/architecture
    - theme: alt
      text: ⭐ GitHub
      link: https://github.com/LessUp/dig-your-windows

features:
  - icon: 🖥️
    title: Hardware Detection
    details: Collect CPU, GPU, RAM, Disk (with SMART), network adapters, USB devices. Real-time monitoring of temperature, load, frequency.
  - icon: 📊
    title: Real-time Monitoring
    details: CPU/GPU temperature, load, frequency, network traffic tracking with ScottPlot chart visualization.
  - icon: 📋
    title: Event Log Analysis
    details: Auto-extract System/Application errors and warnings using EventLogReader with structured XML queries.
  - icon: 📈
    title: Reliability Records
    details: Windows Reliability Monitor data visualization, track application crashes and system issues.
  - icon: ✅
    title: Health Scoring
    details: Comprehensive evaluation across stability, performance, memory, disk health — generates 0-100 score.
  - icon: 💡
    title: Smart Recommendations
    details: AI-generated optimization suggestions based on analysis results.
  - icon: 📄
    title: Report Export
    details: Export diagnostic reports in HTML and JSON formats. HTML reports are self-contained, no internet needed.
  - icon: 🎨
    title: Theme Switching
    details: Dark/light theme toggle with Fluent Design style UI for a modern user experience.
---

## Quick Start

### ⚡ One-Click Install

::: code-group

```powershell [PowerShell]
# Clone the repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

```bash [Bash (WSL/Git Bash)]
# Clone the repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

:::

### 🖥️ System Requirements

| Component | Requirements | Notes |
|-----------|-------------|---------|
| OS | Windows 10/11 | Build 19041+ |
| .NET SDK | 10.0.x | [Download](https://dotnet.microsoft.com/download) |
| Permissions | Admin (recommended) | Required for full feature access |

### 📊 Tech Stack

| Component | Technology | Description |
|-----------|-----------|-------------|
| Runtime | .NET 10.0 + WPF | Desktop application framework |
| UI Library | WPF-UI 4.0 | Fluent Design components |
| MVVM | CommunityToolkit.Mvvm 8.4 | Data binding & commands |
| Charts | ScottPlot 5.1 | Performance trend visualization |
| Hardware Monitor | LibreHardwareMonitor 0.9 | CPU/GPU temperature, load |

## Key Features

### 🔍 Deep Hardware Detection

Comprehensive hardware information including CPU/GPU specifications, disk SMART health data, network configuration and USB device tree.

### 📈 Intelligent Health Scoring

Multi-dimensional algorithm (Stability 40%, Performance 30%, Memory 15%, Disk 15%) generates a system health score with visual grade indicator.

### 🖼️ Intuitive Data Display

Real-time performance charts using ScottPlot, with dark/light theme support for clear data visualization.

::: tip Note
Some features (GPU monitoring, SMART data) require running as administrator for full data access.
:::

## Use Cases

- **🔧 System Maintenance**: Quickly diagnose system issues, identify performance bottlenecks
- **💻 Hardware Upgrades**: Assess current hardware status, plan upgrade paths
- **📊 IT Asset Management**: Batch collect device information, generate unified reports
- **🎯 Technical Support**: Remote diagnostic assistance, rapid problem identification

## Explore Documentation

<div class="vp-doc" style="display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 16px; margin-top: 24px;">

<a href="/en-US/guide/getting-started" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🚀 Getting Started</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Setup, build, run, first-use guide</p>
</div>
</a>

<a href="/en-US/guide/architecture" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🏗️ Architecture</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Tech stack, design patterns, core implementation</p>
</div>
</a>

<a href="/en-US/guide/testing" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🧪 Testing Guide</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Unit tests, property tests, CI/CD integration</p>
</div>
</a>

<a href="/en-US/guide/faq" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">❓ FAQ</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Common questions & troubleshooting</p>
</div>
</a>

</div>

## License

[MIT License](https://github.com/LessUp/dig-your-windows/blob/master/LICENSE) - Copyright © 2025-2026 LessUp
