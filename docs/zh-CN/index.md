---
layout: home

hero:
  name: DigYourWindows
  text: Windows 深度诊断工具
  tagline: 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议
  actions:
    - theme: brand
      text: ⬇️ 立即下载
      link: https://github.com/LessUp/dig-your-windows/releases/latest
    - theme: alt
      text: 📖 使用文档
      link: /zh-CN/guide/getting-started
    - theme: alt
      text: ⭐ GitHub
      link: https://github.com/LessUp/dig-your-windows

features:
  - icon: 🖥️
    title: 硬件信息采集
    details: 全面采集 CPU、GPU、内存、磁盘（含 SMART）、网络适配器、USB 设备信息
  - icon: 📊
    title: 实时监控
    details: CPU/GPU 温度、负载、频率，网络流量实时追踪，ScottPlot 可视化
  - icon: 📋
    title: 事件日志分析
    details: 自动提取 System/Application 错误和警告，结构化 XML 查询高效检索
  - icon: 📈
    title: 可靠性记录
    details: Windows 可靠性监视器数据可视化，追踪系统稳定性趋势
  - icon: ✅
    title: 系统健康评分
    details: 综合评估稳定性、性能、内存、磁盘，生成 0-100 健康评分
  - icon: 💡
    title: 智能优化建议
    details: 基于分析结果自动生成针对性系统优化建议
  - icon: 📄
    title: 报告导出
    details: 支持 HTML/JSON 格式导出，HTML 报告自包含无需网络
  - icon: 🎨
    title: 主题切换
    details: 深色/浅色主题一键切换，Fluent Design 现代 UI
---

<!-- GitHub Stats -->
<div class="stats-section">
  <GitHubStats />
</div>

<!-- Download Section -->
<div class="download-section-container vp-doc">
  <h2>🚀 立即开始使用</h2>
  <DownloadButtons />
</div>

<!-- Quick Start -->
<div class="quick-start-section vp-doc">

## ⚡ 快速安装

### 方式一：下载安装程序（推荐）

点击上方下载按钮获取最新版本，支持三种分发形式：

| 版本类型 | 大小 | 说明 |
|---------|------|------|
| Setup 安装程序 | ~5 MB | 自动下载 .NET 运行时 |
| FDD 框架依赖版 | ~60 MB | 需预先安装 .NET 10 Runtime |
| SCD 独立部署版 | ~180 MB | 无需任何依赖 |

### 方式二：命令行一键安装

::: code-group

```powershell [PowerShell]
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建并运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

```bash [Bash/WSL]
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建并运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

:::

</div>

<!-- System Requirements -->
<div class="requirements-section vp-doc">

## 🖥️ 系统要求

| 组件 | 最低要求 | 说明 |
|------|---------|------|
| 操作系统 | Windows 10/11 | Build 19041+ (20H1) |
| .NET 运行时 | 10.0.x | 仅 FDD 版本需要 |
| 运行权限 | 管理员（推荐） | GPU 监控、SMART 数据需管理员 |
| 磁盘空间 | 200 MB | SCD 版本需求 |

::: tip 💡 提示
部分高级功能（GPU 温度监控、磁盘 SMART 数据）需要以管理员身份运行才能获取完整数据。
:::

</div>

<!-- Tech Stack -->
<div class="tech-stack-section vp-doc">

## 📊 技术栈

<table class="tech-table">
  <tr>
    <th>组件</th>
    <th>技术选型</th>
    <th>说明</th>
  </tr>
  <tr>
    <td>运行框架</td>
    <td><strong>.NET 10.0 + WPF</strong></td>
    <td>桌面应用开发框架</td>
  </tr>
  <tr>
    <td>UI 组件库</td>
    <td><strong>WPF-UI 4.0</strong></td>
    <td>Fluent Design 风格组件</td>
  </tr>
  <tr>
    <td>MVVM 框架</td>
    <td><strong>CommunityToolkit.Mvvm 8.4</strong></td>
    <td>数据绑定与命令</td>
  </tr>
  <tr>
    <td>图表库</td>
    <td><strong>ScottPlot 5.1</strong></td>
    <td>实时性能趋势可视化</td>
  </tr>
  <tr>
    <td>硬件监控</td>
    <td><strong>LibreHardwareMonitor 0.9</strong></td>
    <td>CPU/GPU 温度与负载</td>
  </tr>
</table>

</div>

<!-- Use Cases -->
<div class="use-cases-section vp-doc">

## 🎯 适用场景

<div class="use-cases-grid">
  <div class="use-case-card">
    <div class="use-case-icon">🔧</div>
    <h4>系统维护</h4>
    <p>快速诊断系统问题，识别性能瓶颈，排查故障根源</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">💻</div>
    <h4>硬件升级</h4>
    <p>全面评估当前硬件状态，科学规划升级方案</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">📊</div>
    <h4>IT 资产管理</h4>
    <p>批量采集设备信息，生成标准化诊断报告</p>
  </div>
  <div class="use-case-card">
    <div class="use-case-icon">🎯</div>
    <h4>技术支持</h4>
    <p>远程诊断辅助，快速定位问题，提升支持效率</p>
  </div>
</div>

</div>

<!-- Documentation Links -->
<div class="docs-section vp-doc">

## 📚 使用指南

<div class="docs-grid">
  <a href="/zh-CN/guide/getting-started" class="doc-card">
    <div class="doc-icon">🚀</div>
    <h4>快速开始</h4>
    <p>环境搭建、构建运行、首次使用指南</p>
  </a>
  <a href="/zh-CN/guide/architecture" class="doc-card">
    <div class="doc-icon">🏗️</div>
    <h4>项目架构</h4>
    <p>技术选型、架构设计、核心实现原理</p>
  </a>
  <a href="/zh-CN/guide/testing" class="doc-card">
    <div class="doc-icon">🧪</div>
    <h4>测试指南</h4>
    <p>单元测试、属性测试、CI/CD 集成</p>
  </a>
  <a href="/zh-CN/guide/faq" class="doc-card">
    <div class="doc-icon">❓</div>
    <h4>常见问题</h4>
    <p>常见问题解答与故障排除</p>
  </a>
</div>

</div>

<!-- Security -->
<div class="security-section vp-doc">

## 🔒 安全与隐私

DigYourWindows 完全在本地运行，您的数据安全得到充分保障：

- ✅ **完全离线运行** - 无需网络连接
- ✅ **数据不上传** - 所有诊断数据保存在本地
- ✅ **本地文件系统** - 仅使用本地目录进行数据存储
- ✅ **开源可审计** - 代码完全开源，可自由审查

</div>

<!-- License -->
<div class="license-section vp-doc">

## 📄 开源许可

本项目采用 <a href="https://github.com/LessUp/dig-your-windows/blob/master/LICENSE" target="_blank" rel="noopener"><strong>MIT License</strong></a> 开源协议

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
