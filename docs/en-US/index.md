---
layout: home

hero:
  name: DigYourWindows
  text: Windows Deep Diagnostics
  tagline: One-click hardware info collection, event log analysis, reliability records, system health scoring, and optimization recommendations
  image:
    src: /hero-image.png
    alt: DigYourWindows Interface Preview
  actions:
    - theme: brand
      text: Get Started
      link: /en-US/guide/getting-started
    - theme: alt
      text: Documentation
      link: /en-US/guide/architecture
    - theme: alt
      text: GitHub ⭐
      link: https://github.com/LessUp/dig-your-windows

features:
  - icon: 🖥️
    title: Hardware Information
    details: Comprehensive collection of CPU, GPU, memory, disk (including SMART), network adapters, USB devices, with real-time temperature, load, and frequency monitoring.
  - icon: 📊
    title: Real-time Monitoring
    details: Real-time tracking of CPU temperature/load/frequency, GPU temperature/load/VRAM usage, and network traffic with ScottPlot visualization.
  - icon: 📋
    title: Event Log Analysis
    details: Automatically extract System/Application errors and warnings using EventLogReader with structured XML queries for efficient retrieval.
  - icon: 📈
    title: Reliability Records
    details: Read Windows Reliability Monitor data, visualize system stability trends, and track application failures and system issues.
  - icon: ✅
    title: System Health Score
    details: Comprehensive assessment across stability, performance, memory, and disk health dimensions with auto-generated 0-100 score and grade.
  - icon: 💡
    title: Smart Recommendations
    details: Automatically generate targeted optimization recommendations based on analysis results to help identify and resolve system issues.
  - icon: 📄
    title: Report Export
    details: Export diagnostic reports in HTML and JSON formats. HTML reports are self-contained with embedded styles, viewable offline.
  - icon: 🎨
    title: Theme Support
    details: One-click dark/light theme toggle with Fluent Design style UI for a modern user experience.
---

## Quick Start

### ⚡ One-Command Installation

::: code-group

```powershell [PowerShell]
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

```bash [Bash (WSL/Git Bash)]
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

:::

### 🖥️ System Requirements

| Component | Requirement | Notes |
|-----------|-------------|-------|
| OS | Windows 10/11 | Build 19041+ |
| .NET SDK | 10.0.x | [Download](https://dotnet.microsoft.com/download) |
| Privileges | Admin (Recommended) | Required for full functionality |

### 📊 Tech Stack

| Component | Technology | Purpose |
|-----------|------------|---------|
| Runtime | .NET 10.0 + WPF | Desktop application framework |
| UI Library | WPF-UI 4.0 | Fluent Design components |
| MVVM | CommunityToolkit.Mvvm 8.4 | Data binding and commands |
| Charts | ScottPlot 5.1 | Performance visualization |
| Hardware Monitoring | LibreHardwareMonitor 0.9 | CPU/GPU temperature and load |

## Feature Highlights

### 🔍 Deep Hardware Detection

Collect comprehensive hardware information including CPU/GPU specifications, disk SMART health data, network configuration, and USB device tree.

### 📈 Intelligent Health Scoring

Generate system health scores based on multi-dimensional algorithm (Stability 40%, Performance 30%, Memory 15%, Disk 15%) with visual grading.

### 🖼️ Intuitive Data Display

Real-time performance charts using ScottPlot, supporting dark/light themes with all data at a glance.

::: tip Tip
Some features (GPU monitoring, SMART data) require running as administrator to access complete data.
:::

## Use Cases

- **🔧 System Maintenance**: Quickly diagnose system issues and identify performance bottlenecks
- **💻 Hardware Upgrades**: Assess current hardware status and plan upgrade strategies
- **📊 IT Asset Management**: Batch collect device information and generate unified reports
- **🎯 Technical Support**: Remote diagnostic assistance for quick problem identification

## Documentation

<div class="vp-doc" style="display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 16px; margin-top: 24px;">

<a href="/en-US/guide/getting-started" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🚀 Getting Started</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Environment setup, build, run, and first-time usage guide</p>
</div>
</a>

<a href="/en-US/guide/architecture" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🏗️ Architecture</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Tech stack, architecture design, and core implementation</p>
</div>
</a>

<a href="/en-US/guide/testing" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🧪 Testing Guide</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Unit tests, property-based tests, and CI/CD integration</p>
</div>
</a>

<a href="/en-US/guide/faq" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">❓ FAQ</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">Frequently asked questions and troubleshooting</p>
</div>
</a>

</div>

## License

[MIT License](https://github.com/LessUp/dig-your-windows/blob/master/LICENSE) - Copyright © 2025-2026 LessUp
