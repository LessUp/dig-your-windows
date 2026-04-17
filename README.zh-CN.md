# DigYourWindows

[![CI](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)
[![Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Release](https://img.shields.io/github/v/release/LessUp/dig-your-windows)](https://github.com/LessUp/dig-your-windows/releases)
[![Downloads](https://img.shields.io/github/downloads/LessUp/dig-your-windows/total)](https://github.com/LessUp/dig-your-windows/releases)

[English](README.md) | 简体中文

> 📖 **文档站**：[https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

**Windows 深度诊断工具** — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议。

## 📸 界面预览

![DigYourWindows 主界面](docs/public/screenshot-overview.png)

## ✨ 功能特性

| 功能 | 说明 |
|------|------|
| 🔍 **硬件信息采集** | CPU、GPU、内存、磁盘（含 SMART）、网络适配器、USB 设备 |
| 📊 **实时监控** | CPU 温度/负载/频率、GPU 温度/负载/显存、网络流量 |
| 📋 **事件日志分析** | 自动提取 System/Application 错误和警告日志 |
| 📈 **可靠性记录** | Windows 可靠性监视器数据可视化趋势 |
| ✅ **健康评分** | 综合评估稳定性、性能、内存、磁盘健康 |
| 💡 **智能建议** | 根据分析结果自动生成针对性优化建议 |
| 📄 **报告导出** | 支持 HTML 和 JSON 格式导出，离线查看 |
| 🎨 **主题切换** | 深色/浅色主题一键切换 |

## 🚀 快速开始

### 环境要求

- Windows 10/11 (Build 19041+)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)（构建需要）

### 安装方式

#### 方式一：下载安装包（推荐）

从 [Releases](https://github.com/LessUp/dig-your-windows/releases) 下载最新版本：

| 版本 | 大小 | 环境要求 |
|------|------|----------|
| `DigYourWindows_Setup.exe` | ~5MB | 自动下载 .NET（如需要）|
| FDD（框架依赖版） | ~60MB | 需要 .NET 10 Runtime |
| SCD（独立版） | ~180MB | 无需依赖 |

#### 方式二：源码构建

```powershell
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建并运行
dotnet restore
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ **提示**：部分功能（GPU 监控、SMART 数据）需要以管理员权限运行。

### 首次使用

1. 启动 `DigYourWindows.UI.exe`（或通过 `dotnet run` 运行）
2. 点击"运行诊断"采集系统信息
3. 在主界面查看实时硬件监控
4. 通过 JSON 或 HTML 格式导出报告

## 🏗️ 技术栈

| 组件 | 技术 | 版本 | 用途 |
|------|------|------|------|
| 运行时 | .NET + WPF | 10.0 | 桌面应用框架 |
| UI 库 | WPF-UI | 4.0 | Fluent Design 组件 |
| MVVM | CommunityToolkit.Mvvm | 8.4 | 数据绑定与命令 |
| 图表 | ScottPlot | 5.1 | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor | 0.9 | CPU/GPU 温度、负载、频率 |
| 测试 | xUnit + FsCheck | 2.9 / 2.16 | 单元测试 + 属性测试 |

## 📁 项目结构

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # 核心业务逻辑
│   └── DigYourWindows.UI/       # WPF 用户界面
├── tests/
│   └── DigYourWindows.Tests/    # 单元测试和属性测试
├── specs/                       # 规范文档（SDD）
│   ├── product/                 # 产品需求 (PRD)
│   ├── rfc/                     # 技术设计文档
│   ├── api/                     # API 规范
│   ├── db/                      # 数据模型规范
│   └── testing/                 # 测试策略和 BDD 规范
├── docs/                        # VitePress 文档站点
│   ├── zh-CN/                   # 中文文档
│   ├── en-US/                   # 英文文档
│   ├── public/                  # 静态资源（图片、图标）
│   └── .vitepress/              # VitePress 配置
├── installer/                   # Inno Setup 安装脚本
└── scripts/                     # 构建和发布脚本
```

## 🧪 测试

```powershell
# 运行所有测试
dotnet test DigYourWindows.slnx

# 运行并收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 运行特定测试
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

## 📚 文档

- [📖 快速开始指南](https://lessup.github.io/dig-your-windows/zh-CN/guide/getting-started)
- [🏗️ 架构文档](https://lessup.github.io/dig-your-windows/zh-CN/guide/architecture)
- [🧪 测试指南](https://lessup.github.io/dig-your-windows/zh-CN/guide/testing)
- [📊 数据 Schema 参考](https://lessup.github.io/dig-your-windows/zh-CN/reference/data-schema)
- [❓ 常见问题](https://lessup.github.io/dig-your-windows/zh-CN/guide/faq)

文档提供 **English** 和 **简体中文** 两种语言。

## 🤝 贡献指南

欢迎贡献！本项目遵循**规范驱动开发（SDD）**工作流。

### 快速开始

1. 阅读 [CONTRIBUTING.md](CONTRIBUTING.md) 了解详细指南
2. 开始工作前查看现有的[规范文档](specs/)
3. 为新功能创建或更新规范
4. 按照规范实现代码并提交 Pull Request

### 贡献流程

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. **查看/创建规范** 在 `/specs/` 目录中
4. 按照规范实现代码
5. 根据验收标准编写测试
6. 提交更改 (`git commit -m 'feat: add amazing feature'`)
7. 推送到分支 (`git push origin feature/amazing-feature`)
8. 创建 Pull Request

查看 [CONTRIBUTING.md](CONTRIBUTING.md) 获取完整指南，包括 SDD 工作流说明。

## 📦 发布流程

推送 `v*` 格式的 tag 触发自动发布：

```powershell
git tag v1.1.0
git push origin v1.1.0
```

GitHub Actions 将自动构建并发布双语 Release 说明。

## 🔒 安全说明

本工具：
- ✅ 完全离线运行
- ✅ 不会上传任何数据到外部服务器
- ✅ 所有操作使用本地文件系统
- ✅ 开源可审计

## 📄 许可证

[MIT License](LICENSE) - Copyright © 2025-2026 LessUp

---

<p align="center">
  用 ❤️ 制作 by <a href="https://github.com/LessUp">LessUp</a>
</p>
