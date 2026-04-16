---
layout: home

hero:
  name: DigYourWindows
  text: Windows 深度诊断工具
  tagline: 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议
  image:
    src: /hero-image.png
    alt: DigYourWindows 界面预览
  actions:
    - theme: brand
      text: 快速开始
      link: /zh-CN/guide/getting-started
    - theme: alt
      text: 查看文档
      link: /zh-CN/guide/architecture
    - theme: alt
      text: GitHub ⭐
      link: https://github.com/LessUp/dig-your-windows

features:
  - icon: 🖥️
    title: 硬件信息采集
    details: 全面采集 CPU、GPU、内存、磁盘（含 SMART 数据）、网络适配器、USB 设备等硬件信息，支持实时监控温度、负载、频率。
  - icon: 📊
    title: 实时监控
    details: CPU 温度/负载/频率、GPU 温度/负载/显存使用、网络流量实时追踪，ScottPlot 图表可视化趋势。
  - icon: 📋
    title: 事件日志分析
    details: 自动提取 System/Application 日志中的错误和警告事件，使用 EventLogReader + 结构化 XML 查询高效检索。
  - icon: 📈
    title: 可靠性记录
    details: 读取 Windows 可靠性监视器数据，可视化展示系统稳定性趋势，追踪应用程序故障和系统问题。
  - icon: ✅
    title: 系统健康评分
    details: 综合评估稳定性、性能、内存、磁盘健康四个维度，自动生成 0-100 分的系统健康评分和等级。
  - icon: 💡
    title: 智能优化建议
    details: 根据分析结果自动生成针对性的优化建议，帮助用户识别和解决系统问题。
  - icon: 📄
    title: 报告导出
    details: 支持 HTML 和 JSON 两种格式导出诊断报告，HTML 报告自包含样式，无需网络连接即可查看。
  - icon: 🎨
    title: 主题切换
    details: 支持深色/浅色主题一键切换，Fluent Design 风格 UI，提供现代化的用户体验。
---

## 快速开始

### ⚡ 一键安装

::: code-group

```powershell [PowerShell]
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建并运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

```bash [Bash (WSL/Git Bash)]
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建并运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

:::

### 🖥️ 系统要求

| 组件 | 要求 | 说明 |
|------|------|------|
| 操作系统 | Windows 10/11 | Build 19041+ |
| .NET SDK | 10.0.x | [下载](https://dotnet.microsoft.com/download) |
| 权限 | 管理员（推荐） | 完整功能需要 |

### 📊 技术栈

| 组件 | 技术 | 说明 |
|------|------|------|
| 运行时 | .NET 10.0 + WPF | 桌面应用框架 |
| UI 库 | WPF-UI 4.0 | Fluent Design 组件 |
| MVVM | CommunityToolkit.Mvvm 8.4 | 数据绑定与命令 |
| 图表 | ScottPlot 5.1 | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor 0.9 | CPU/GPU 温度、负载 |

## 功能亮点

### 🔍 深度硬件检测

采集全面的硬件信息，包括 CPU/GPU 详细规格、磁盘 SMART 健康数据、网络配置和 USB 设备树。

### 📈 智能健康评分

基于多维算法（稳定性 40%、性能 30%、内存 15%、磁盘 15%）生成系统健康评分，并给出可视化等级。

### 🖼️ 直观数据展示

使用 ScottPlot 实现实时性能图表，支持深色/浅色主题，所有数据一目了然。

::: tip 提示
部分功能（GPU 监控、SMART 数据）需要以管理员身份运行才能获取完整数据。
:::

## 应用场景

- **🔧 系统维护**: 快速诊断系统问题，识别性能瓶颈
- **💻 硬件升级**: 评估当前硬件状态，规划升级方案
- **📊 IT 资产管理**: 批量采集设备信息，生成统一报告
- **🎯 技术支持**: 远程诊断辅助，快速定位问题

## 使用指南

<div class="vp-doc" style="display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 16px; margin-top: 24px;">

<a href="/zh-CN/guide/getting-started" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🚀 快速开始</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">环境搭建、构建运行、首次使用指南</p>
</div>
</a>

<a href="/zh-CN/guide/architecture" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🏗️ 项目架构</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">技术选型、架构设计、核心实现</p>
</div>
</a>

<a href="/zh-CN/guide/testing" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">🧪 测试指南</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">单元测试、属性测试、CI/CD 集成</p>
</div>
</a>

<a href="/zh-CN/guide/faq" style="text-decoration: none;">
<div style="padding: 16px; border: 1px solid var(--vp-c-divider); border-radius: 8px; transition: all 0.3s;">
<h4 style="margin: 0 0 8px; color: var(--vp-c-brand-1);">❓ FAQ</h4>
<p style="margin: 0; color: var(--vp-c-text-2);">常见问题解答与故障排除</p>
</div>
</a>

</div>

## 许可证

[MIT License](https://github.com/LessUp/dig-your-windows/blob/master/LICENSE) - Copyright © 2025-2026 LessUp
