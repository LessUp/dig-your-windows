---
layout: home

hero:
  name: DigYourWindows
  text: Windows 深度诊断工具
  tagline: 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议
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
  - title: 硬件信息采集
    details: CPU、GPU、内存、磁盘（含 SMART）、网络适配器、USB 设备全面采集
    icon: 🖥️
  - title: 实时监控
    details: CPU 温度/负载/频率、GPU 温度/负载/显存、网络流量实时追踪
    icon: 📊
  - title: 事件日志分析
    details: 自动提取 System/Application 错误和警告，使用 EventLogReader + 结构化 XML 查询高效检索
    icon: 📋
  - title: 系统健康评分
    details: 综合评估稳定性、性能、内存、磁盘健康（0–100 分），自动生成针对性优化建议
    icon: ✅
  - title: 可靠性记录
    details: 读取 Windows 可靠性监视器数据并可视化趋势，追踪系统稳定性变化
    icon: 📈
  - title: 报告导出
    details: 支持 HTML 和 JSON 格式导出，深色/浅色主题一键切换
    icon: 📄
---

## 技术栈

| 组件 | 技术 | 说明 |
|------|------|------|
| 运行时 | .NET 10.0 + WPF | 桌面应用框架 |
| UI 库 | WPF-UI | Fluent Design 风格 |
| MVVM | CommunityToolkit.Mvvm | 数据绑定与命令 |
| 图表 | ScottPlot | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor | CPU/GPU 温度、负载、频率 |
| 测试 | xUnit + FsCheck | 单元测试 + 属性测试 |

## 架构亮点

- **共享 `Directory.Build.props`** — TargetFramework、Nullable、版本号等属性统一管理
- **共享硬件监控实例** — `HardwareMonitorProvider` 单例，CPU/GPU 服务共享同一 `Computer` 实例
- **高效事件日志** — `EventLogReader` + 结构化 XML 查询，替代全量遍历
- **CancellationToken 全链路** — 所有耗时操作均可取消，保证 UI 响应性
- **缓冲日志** — `StreamWriter` 持久写入，替代逐次 `File.AppendAllText`
