---
layout: home

hero:
  name: DigYourWindows
  text: Windows 深度诊断工具
  tagline: 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议
  image:
    src: /hero-image.png
    alt: DigYourWindows 截图
  actions:
    - theme: brand
      text: 快速开始
      link: /guide/getting-started
    - theme: alt
      text: 项目架构
      link: /guide/architecture
    - theme: alt
      text: GitHub
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
    title: 优化建议
    details: 根据分析结果自动生成针对性的优化建议，帮助用户识别和解决系统问题。
  - icon: 📄
    title: 报告导出
    details: 支持 HTML 和 JSON 两种格式导出诊断报告，HTML 报告自包含样式，无需网络连接即可查看。
  - icon: 🎨
    title: 主题切换
    details: 支持深色/浅色主题一键切换，Fluent Design 风格 UI，提供现代化的用户体验。
---

## 快速开始

```powershell
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 构建项目
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release

# 运行应用
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

## 技术栈

| 组件 | 技术 | 说明 |
|------|------|------|
| 运行时 | .NET 10.0 + WPF | 桌面应用框架 |
| UI 库 | WPF-UI 4.0 | Fluent Design 风格组件 |
| MVVM | CommunityToolkit.Mvvm 8.4 | 数据绑定与命令 |
| 图表 | ScottPlot 5.1 | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor 0.9 | CPU/GPU 温度、负载、频率 |
| 测试 | xUnit 2.9 + FsCheck 2.16 | 单元测试 + 属性测试 |

## 架构亮点

- **共享构建属性** — `Directory.Build.props` 统一管理 TargetFramework、Nullable、版本号
- **单例硬件监控** — `HardwareMonitorProvider` 共享 LibreHardwareMonitor 实例
- **高效事件日志** — `EventLogReader` + 结构化 XML 查询，服务端过滤
- **CancellationToken 支持** — 所有 I/O 密集型操作均可取消
- **缓冲日志写入** — `StreamWriter` + 日志轮转，高效 I/O
- **接口依赖注入** — 所有服务都有接口，便于测试

## 许可证

[MIT License](https://github.com/LessUp/dig-your-windows/blob/master/LICENSE) - Copyright © 2025-2026 LessUp
