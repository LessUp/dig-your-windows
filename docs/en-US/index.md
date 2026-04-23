---
layout: home

hero:
  name: DigYourWindows
  text: Windows Deep Diagnostics Tool
  tagline: One-click hardware information collection, event log analysis, reliability records, system health scoring & optimization recommendations
  actions:
    - theme: brand
      text: ⬇️ Download Now
      link: https://github.com/LessUp/dig-your-windows/releases/latest
    - theme: alt
      text: 📖 Documentation
      link: /en-US/guide/getting-started
    - theme: alt
      text: ⭐ GitHub
      link: https://github.com/LessUp/dig-your-windows

features:
  - icon: 🖥️
    title: Hardware Detection
    details: Collect CPU, GPU, RAM, Disk (with SMART), Network adapters, USB devices comprehensively
  - icon: 📊
    title: Real-time Monitoring
    details: CPU/GPU temperature, load, frequency, network traffic with ScottPlot visualization
  - icon: 📋
    title: Event Log Analysis
    details: Auto-extract System/Application errors and warnings with structured XML queries
  - icon: 📈
    title: Reliability Records
    details: Windows Reliability Monitor data visualization with stability trend tracking
  - icon: ✅
    title: Health Scoring
    details: Comprehensive assessment across stability, performance, memory, disk - 0-100 score
  - icon: 💡
    title: Smart Recommendations
    details: AI-generated optimization suggestions based on analysis results
  - icon: 📄
    title: Report Export
    details: Export to HTML/JSON formats - HTML reports are self-contained for offline viewing
  - icon: 🎨
    title: Theme Support
    details: Dark/Light theme toggle with Fluent Design modern UI
---

<!-- GitHub Stats -->
<div class="stats-section">
  <GitHubStats />
</div>

<!-- Download Section -->
<div class="download-section-container vp-doc">
  <h2>🚀 Get Started Now</h2>
  <DownloadButtons />
</div>

<!-- Quick Start -->
<div class="quick-start-section vp-doc">

## ⚡ Quick Installation

### Option 1: Download Installer (Recommended)

Click the download button above to get the latest release. Three distribution formats available:

| Version Type | Size | Description |
|-------------|------|-------------|
| Setup Installer | ~5 MB | Auto-downloads .NET runtime if needed |
| FDD (Framework-Dependent) | ~60 MB | Requires .NET 10 Runtime pre-installed |
| SCD (Self-Contained) | ~180 MB | No dependencies required |

### Option 2: Command Line Installation

::: code-group

```powershell [PowerShell]
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

```bash [Bash/WSL]
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

:::

</div>

<!-- System Requirements -->
<div class="requirements-section vp-doc">

## 🖥️ System Requirements

| Component | Minimum | Notes |
|-----------|---------|-------|
| OS | Windows 10/11 | Build 19041+ (20H1) |
| .NET Runtime | 10.0.x | Required for FDD version only |
| Privileges | Administrator (recommended) | GPU monitoring, SMART data need admin |
| Disk Space | 200 MB | For SCD version |

::: tip 💡 Tip
Some advanced features (GPU temperature monitoring, disk SMART data) require administrator privileges for full data access.
:::

</div>

<!-- Tech Stack -->
<div class="tech-stack-section vp-doc">

## 📊 Tech Stack

<table class="tech-table">
  <tr>
    <th>Component</th>
    <th>Technology</th>
    <th>Description</th>
  </tr>
  <tr>
    <td>Runtime</td>
    <td><strong>.NET 10.0 + WPF</strong></td>
    <td>Desktop application framework</td>
  </tr>
  <tr>
    <td>UI Library</td>
    <td><strong>WPF-UI 4.0</strong></td>
    <td>Fluent Design style components</td>
  </tr>
  <tr>
    <td>MVVM Framework</td>
    <td><strong>CommunityToolkit.Mvvm 8.4</strong></td>
    <td>Data binding and commands</td>
  </tr>
  <tr>
    <td>Charting</td>
    <td><strong>ScottPlot 5.1</strong></td>
    <td>Real-time performance visualization</td>
  </tr>
  <tr>
    <td>Hardware Monitoring</td>
    <td><strong>LibreHardwareMonitor 0.9</strong></td>
    <td>CPU/GPU temperature and load</td>
  </tr>
</table>

</div>

<!-- Use Cases -->
<div class="use-cases-section vp-doc">

## 🎯 Use Cases

<div class="use-cases-grid">
  <div class="use-case-card">
    <div class="use-case-icon">🔧</div>
    <h4>System Maintenance</h4>
    <p>Quickly diagnose system issues, identify performance bottlenecks</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">💻</div>
    <h4>Hardware Upgrades</h4>
    <p>Comprehensively assess current hardware status, plan upgrades scientifically</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">📊</div>
    <h4>IT Asset Management</h4>
    <p>Batch collect device information, generate standardized diagnostic reports</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">🎯</div>
    <h4>Technical Support</h4>
    <p>Remote diagnostic assistance, rapid problem identification</p>
  </div>
</div>

</div>

<!-- Documentation Links -->
<div class="docs-section vp-doc">

## 📚 Documentation

<div class="docs-grid">
  <a href="/en-US/guide/getting-started" class="doc-card">
    <div class="doc-icon">🚀</div>
    <h4>Getting Started</h4>
    <p>Environment setup, build, run, first-use guide</p>
  </a>
  <a href="/en-US/guide/architecture" class="doc-card">
    <div class="doc-icon">🏗️</div>
    <h4>Architecture</h4>
    <p>Technology choices, design patterns, core implementation</p>
  </a>
  <a href="/en-US/guide/testing" class="doc-card">
    <div class="doc-icon">🧪</div>
    <h4>Testing Guide</h4>
    <p>Unit tests, property tests, CI/CD integration</p>
  </a>
  <a href="/en-US/guide/faq" class="doc-card">
    <div class="doc-icon">❓</div>
    <h4>FAQ</h4>
    <p>Common questions and troubleshooting</p>
  </a>
</div>

</div>

<!-- Security -->
<div class="security-section vp-doc">

## 🔒 Security & Privacy

DigYourWindows runs entirely on your local machine - your data is fully protected:

- ✅ **Fully Offline** - No network connection required
- ✅ **No Data Upload** - All diagnostic data stays on your device
- ✅ **Local File System** - Only uses local directories for data storage
- ✅ **Open Source & Auditable** - Code is fully open source for review

</div>

<!-- License -->
<div class="license-section vp-doc">

## 📄 Open Source License

This project is licensed under the <a href="https://github.com/LessUp/dig-your-windows/blob/master/LICENSE" target="_blank" rel="noopener"><strong>MIT License</strong></a>

Copyright © 2025-2026 <a href="https://github.com/LessUp" target="_blank" rel="noopener">LessUp</a>

</div>

<style scoped>
/* Stats Section */
.stats-section {
  max-width: 600px;
  margin: 1rem auto 2rem;
  padding: 0 1.5rem;
}

/* Download Section */
.download-section-container {
  text-align: center;
  padding: 2rem 1.5rem;
  background: var(--vp-c-bg-soft);
  border-radius: 16px;
  margin: 2rem auto;
  max-width: 800px;
}

.download-section-container h2 {
  margin-top: 0;
  margin-bottom: 1.5rem;
}

/* Quick Start */
.quick-start-section {
  max-width: 800px;
  margin: 3rem auto;
  padding: 0 1.5rem;
}

/* Requirements */
.requirements-section {
  max-width: 800px;
  margin: 3rem auto;
  padding: 0 1.5rem;
}

.requirements-section table {
  width: 100%;
}

/* Tech Stack */
.tech-stack-section {
  max-width: 900px;
  margin: 3rem auto;
  padding: 0 1.5rem;
}

.tech-table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
}

.tech-table th {
  background: var(--vp-c-bg-soft);
  font-weight: 600;
  text-align: left;
  padding: 0.75rem 1rem;
}

.tech-table td {
  padding: 0.75rem 1rem;
  border-bottom: 1px solid var(--vp-c-divider);
}

.tech-table tr:hover td {
  background: var(--vp-c-bg-soft);
}

/* Use Cases */
.use-cases-section {
  max-width: 1000px;
  margin: 3rem auto;
  padding: 0 1.5rem;
}

.use-cases-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1.5rem;
  margin-top: 1.5rem;
}

.use-case-card {
  text-align: center;
  padding: 1.5rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 12px;
  transition: all 0.3s ease;
}

.use-case-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 40px -12px rgba(0, 0, 0, 0.15);
  border-color: var(--vp-c-brand-1);
}

.use-case-icon {
  font-size: 2.5rem;
  margin-bottom: 0.75rem;
}

.use-case-card h4 {
  margin: 0 0 0.5rem;
  color: var(--vp-c-text-1);
}

.use-case-card p {
  margin: 0;
  font-size: 0.9rem;
  color: var(--vp-c-text-2);
}

/* Documentation */
.docs-section {
  max-width: 1000px;
  margin: 3rem auto;
  padding: 0 1.5rem;
}

.docs-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
  margin-top: 1.5rem;
}

.doc-card {
  display: block;
  padding: 1.25rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 12px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.doc-card:hover {
  background: var(--vp-c-bg);
  border-color: var(--vp-c-brand-1);
  transform: translateY(-2px);
}

.doc-icon {
  font-size: 1.75rem;
  margin-bottom: 0.5rem;
}

.doc-card h4 {
  margin: 0 0 0.35rem;
  color: var(--vp-c-brand-1);
  font-size: 1rem;
}

.doc-card p {
  margin: 0;
  font-size: 0.85rem;
  color: var(--vp-c-text-2);
}

/* Security & License */
.security-section,
.license-section {
  max-width: 800px;
  margin: 3rem auto;
  padding: 0 1.5rem;
  text-align: center;
}

.license-section {
  padding-bottom: 2rem;
}

@media (max-width: 640px) {
  .download-section-container {
    margin: 1.5rem;
  }
  
  .use-cases-grid {
    grid-template-columns: 1fr;
  }
  
  .docs-grid {
    grid-template-columns: 1fr;
  }
}
</style>
