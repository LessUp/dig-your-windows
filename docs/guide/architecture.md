# 项目架构

## 技术栈

| 组件 | 技术 |
|------|------|
| 框架 | .NET 10.0 + WPF |
| UI 库 | WPF-UI (Fluent Design) |
| MVVM | CommunityToolkit.Mvvm |
| 图表 | ScottPlot |
| 硬件监控 | LibreHardwareMonitor |
| 测试 | xUnit + FsCheck |

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
├── docs/                        # 文档站（VitePress）
├── installer/                   # Inno Setup 安装包脚本
├── scripts/                     # 构建和发布脚本
├── changelog/                   # 变更日志
├── .editorconfig                # 代码风格规范
├── Directory.Build.props        # 共享 MSBuild 属性
├── DigYourWindows.slnx          # Solution 文件
└── NuGet.Config
```

## 架构要点

### 共享 `Directory.Build.props`

TargetFramework、Nullable、版本号等属性统一管理，避免各 `.csproj` 文件重复配置。

### 共享硬件监控实例

`HardwareMonitorProvider` 单例管理 LibreHardwareMonitor 的 `Computer` 实例，CPU/GPU 监控服务共享同一实例，避免资源浪费。

### 高效事件日志读取

使用 `EventLogReader` + 结构化 XML 查询，替代遍历全部日志条目，大幅提升性能。

### CancellationToken 支持

硬件采集、事件日志、可靠性记录等耗时操作均支持取消，保证 UI 响应性。

### 模型拆分

数据模型按职责拆分为独立文件：

- `DiagnosticData` — 诊断数据总览
- `HardwareData` — 硬件信息
- `DiskModels` — 磁盘与 SMART
- `DeviceModels` — 设备信息
- `ComputeModels` — 计算性能
- `EventModels` — 事件日志
- `PerformanceAnalysisData` — 性能分析

### 缓冲日志

`FileLogService` 使用 `StreamWriter` 替代逐次 `File.AppendAllText`，减少 I/O 开销。
