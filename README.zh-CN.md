# DigYourWindows

[![CI](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/ci.yml)
[![Docs](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml/badge.svg)](https://github.com/LessUp/dig-your-windows/actions/workflows/pages.yml)

[English](README.md) | 简体中文

> 📖 **文档站**：[https://lessup.github.io/dig-your-windows/](https://lessup.github.io/dig-your-windows/)

Windows 深度诊断工具 — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议。

## 功能

- **硬件信息采集** — CPU、GPU、内存、磁盘（含 SMART）、网络适配器、USB 设备
- **实时监控** — CPU 温度/负载/频率、GPU 温度/负载/显存、网络流量
- **事件日志分析** — 自动提取 System/Application 错误和警告日志
- **可靠性记录** — 读取 Windows 可靠性监视器数据并可视化趋势
- **系统健康评分** — 综合评估稳定性、性能、内存、磁盘健康
- **优化建议** — 根据分析结果自动生成针对性建议
- **报告导出** — 支持 HTML 和 JSON 格式导出
- **深色/浅色主题** — 支持一键切换

## 技术栈

- **.NET 10.0** + **WPF**
- **WPF-UI** (Fluent Design)
- **CommunityToolkit.Mvvm**
- **ScottPlot** (图表)
- **LibreHardwareMonitor** (硬件监控)
- **xUnit** + **FsCheck** (测试)

## 项目结构

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # 核心业务逻辑
│   │   ├── Models/              # 数据模型（按类型拆分）
│   │   ├── Services/            # 服务层（硬件、事件、性能分析等）
│   │   └── Exceptions/          # 自定义异常
│   └── DigYourWindows.UI/       # WPF 用户界面
├── tests/
│   └── DigYourWindows.Tests/    # 单元测试和属性测试
├── docs/                        # 文档和数据 Schema
├── installer/                   # Inno Setup 安装包脚本
├── scripts/                     # 构建和发布脚本
├── changelog/                   # 变更日志
├── .editorconfig                # 代码风格规范
├── Directory.Build.props        # 共享 MSBuild 属性
├── DigYourWindows.slnx          # Solution 文件
└── NuGet.Config
```

## 快速开始

### 环境要求

- Windows 10/11
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### 构建

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release
```

### 运行

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> 部分功能（如 GPU 监控、SMART 数据）需要以管理员权限运行。

### 测试

```powershell
dotnet test DigYourWindows.slnx
```

### 发布

```powershell
# 使用发布脚本（生成 FDD 和 SCD 两个版本）
.\scripts\publish.ps1

# 构建安装包（需要 Inno Setup 6）
.\scripts\build-installer.ps1
```

## Release

推送 `v*` 格式的 tag 会自动触发 GitHub Actions 构建并发布 Release：

```powershell
git tag v1.0.0
git push origin v1.0.0
```

## 架构要点

- **共享 `Directory.Build.props`** — TargetFramework、Nullable、版本号等属性统一管理
- **`.editorconfig`** — 统一代码风格（缩进、命名、using 排序等）
- **共享硬件监控实例** — `HardwareMonitorProvider` 单例管理 LibreHardwareMonitor `Computer`，CPU/GPU 监控共享
- **高效事件日志读取** — 使用 `EventLogReader` + 结构化 XML 查询，替代遍历全部条目
- **CancellationToken 支持** — 硬件采集、事件日志、可靠性记录均支持取消
- **模型拆分** — 数据模型按职责拆分为独立文件（DiagnosticData、HardwareData、DiskModels 等）
- **缓冲日志** — `FileLogService` 使用 `StreamWriter` 替代逐次 `File.AppendAllText`

## 许可证

[MIT License](LICENSE)
