# DigYourWindows

[![CI](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)
[![Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

[English](README.md) | 简体中文

> 📖 **文档站**：[https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

**Windows 深度诊断工具** — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议。

## 功能特性

| 功能 | 说明 |
|------|------|
| 🔍 **硬件信息采集** | CPU、GPU、内存、磁盘（含 SMART）、网络适配器、USB 设备 |
| 📊 **实时监控** | CPU 温度/负载/频率、GPU 温度/负载/显存、网络流量 |
| 📋 **事件日志分析** | 自动提取 System/Application 错误和警告日志 |
| 📈 **可靠性记录** | Windows 可靠性监视器数据可视化趋势 |
| ✅ **健康评分** | 综合评估稳定性、性能、内存、磁盘健康 |
| 💡 **优化建议** | 根据分析结果自动生成针对性建议 |
| 📄 **报告导出** | 支持 HTML 和 JSON 格式导出 |
| 🎨 **主题切换** | 深色/浅色主题一键切换 |

## 技术栈

| 组件 | 技术 | 用途 |
|------|------|------|
| 运行时 | .NET 10.0 + WPF | 桌面应用框架 |
| UI 库 | WPF-UI 4.0 | Fluent Design 风格组件 |
| MVVM | CommunityToolkit.Mvvm 8.4 | 数据绑定与命令 |
| 图表 | ScottPlot 5.1 | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor 0.9 | CPU/GPU 温度、负载、频率 |
| 测试 | xUnit 2.9 + FsCheck 2.16 | 单元测试 + 属性测试 |

## 快速开始

### 环境要求

- Windows 10/11
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### 构建与运行

```powershell
# 还原依赖
dotnet restore DigYourWindows.slnx

# 构建
dotnet build DigYourWindows.slnx -c Release

# 运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ 部分功能（GPU 监控、SMART 数据）需要以管理员权限运行。

### 测试

```powershell
# 运行所有测试
dotnet test DigYourWindows.slnx

# 运行并收集覆盖率
dotnet test DigYourWindows.slnx --collect:"XPlat Code Coverage"
```

### 发布

```powershell
# 生成框架依赖和自包含两个版本
.\scripts\publish.ps1

# 构建安装包（需要 Inno Setup 6）
.\scripts\build-installer.ps1
```

## 项目结构

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # 核心业务逻辑
│   │   ├── Models/              # 数据模型（按领域拆分）
│   │   ├── Services/            # 服务层
│   │   └── Exceptions/          # 自定义异常
│   └── DigYourWindows.UI/       # WPF 用户界面
│       ├── ViewModels/          # MVVM 视图模型
│       └── Converters/          # 值转换器
├── tests/
│   └── DigYourWindows.Tests/    # 单元测试和属性测试
│       ├── Unit/                # 单元测试
│       └── Property/            # 属性测试
├── docs/                        # VitePress 文档站
├── installer/                   # Inno Setup 安装脚本
├── scripts/                     # 构建和发布脚本
├── changelog/                   # 变更日志
├── Directory.Build.props        # 共享 MSBuild 属性
└── DigYourWindows.slnx          # Solution 文件
```

## 架构要点

- **共享构建属性** — `Directory.Build.props` 统一管理 TargetFramework、Nullable、版本号
- **单例硬件监控** — `HardwareMonitorProvider` 共享 LibreHardwareMonitor 实例
- **高效事件日志** — `EventLogReader` + 结构化 XML 查询，服务端过滤
- **CancellationToken 支持** — 所有 I/O 密集型操作均可取消
- **缓冲日志写入** — `StreamWriter` + 日志轮转，高效 I/O
- **接口依赖注入** — 所有服务都有接口，便于测试

## 发布流程

推送 `v*` 格式的 tag 会自动触发 GitHub Actions 构建并发布：

```powershell
git tag v1.0.0
git push origin v1.0.0
```

## 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. 提交更改 (`git commit -m 'Add amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 创建 Pull Request

## 许可证

[MIT License](LICENSE) - Copyright © 2025-2026 LessUp
