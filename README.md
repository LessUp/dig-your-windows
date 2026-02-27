# DigYourWindows

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
│   ├── DigYourWindows.Core/     # 核心业务逻辑、模型、服务
│   └── DigYourWindows.UI/       # WPF 用户界面
├── tests/
│   └── DigYourWindows.Tests/    # 单元测试和属性测试
├── docs/                        # 文档和数据 Schema
├── installer/                   # Inno Setup 安装包脚本
├── scripts/                     # 构建和发布脚本
├── changelog/                   # 变更日志
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

## 许可证

[MIT License](LICENSE)
