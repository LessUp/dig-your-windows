# 项目架构

本文档详细介绍 DigYourWindows 的技术架构和设计决策。

## 技术栈

| 组件 | 技术 | 版本 | 用途 |
|------|------|------|------|
| 运行时 | .NET + WPF | 10.0 | 桌面应用框架 |
| UI 库 | WPF-UI | 4.0 | Fluent Design 风格组件 |
| MVVM | CommunityToolkit.Mvvm | 8.4 | 数据绑定与命令 |
| 图表 | ScottPlot | 5.1 | 性能趋势可视化 |
| 硬件监控 | LibreHardwareMonitor | 0.9 | CPU/GPU 温度、负载、频率 |
| WMI | System.Management | 10.0 | Windows 管理信息查询 |
| 测试 | xUnit + FsCheck | 2.9 / 2.16 | 单元测试 + 属性测试 |

## 项目结构

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # 核心业务逻辑
│   │   ├── Models/              # 数据模型（按领域拆分）
│   │   │   ├── DiagnosticData.cs
│   │   │   ├── HardwareData.cs
│   │   │   ├── DiskModels.cs
│   │   │   ├── ComputeModels.cs
│   │   │   ├── EventModels.cs
│   │   │   ├── DeviceModels.cs
│   │   │   ├── CollectionModels.cs
│   │   │   └── PerformanceAnalysisData.cs
│   │   ├── Services/            # 服务层
│   │   │   ├── HardwareService.cs
│   │   │   ├── CpuMonitorService.cs
│   │   │   ├── GpuMonitorService.cs
│   │   │   ├── NetworkMonitorService.cs
│   │   │   ├── DiskSmartService.cs
│   │   │   ├── EventLogService.cs
│   │   │   ├── ReliabilityService.cs
│   │   │   ├── PerformanceService.cs
│   │   │   ├── ReportService.cs
│   │   │   ├── DiagnosticCollectorService.cs
│   │   │   ├── LogService.cs
│   │   │   ├── HardwareMonitorProvider.cs
│   │   │   └── ScoringConfiguration.cs
│   │   └── Exceptions/          # 自定义异常
│   │       ├── ServiceException.cs
│   │       ├── ReportException.cs
│   │       └── WmiException.cs
│   └── DigYourWindows.UI/       # WPF 用户界面
│       ├── ViewModels/          # MVVM 视图模型
│       │   └── MainViewModel.cs
│       ├── Converters/          # 值转换器
│       │   ├── CountToVisibilityConverter.cs
│       │   ├── NullConverters.cs
│       │   └── StringToBrushConverter.cs
│       ├── App.xaml.cs          # 应用入口 + DI 组合根
│       └── MainWindow.xaml.cs   # 主窗口
├── tests/
│   └── DigYourWindows.Tests/    # 测试项目
│       ├── Unit/                # 单元测试
│       │   ├── ReportServiceTests.cs
│       │   ├── DiagnosticCollectorServiceTests.cs
│       │   └── PerformanceServiceTests.cs
│       ├── Property/            # 属性测试
│       │   └── ReportServicePropertyTests.cs
│       ├── FsCheckConfig.cs     # FsCheck 配置
│       └── Usings.cs            # 全局 using
├── docs/                        # VitePress 文档站
├── installer/                   # Inno Setup 安装脚本
├── scripts/                     # 构建和发布脚本
├── Directory.Build.props        # 共享 MSBuild 属性
└── DigYourWindows.slnx          # Solution 文件
```

## 核心架构设计

### 1. 共享构建属性 (`Directory.Build.props`)

集中管理所有项目的通用 MSBuild 属性：

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0-windows10.0.19041.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
  </PropertyGroup>
  <!-- Version, Authors, Product, etc. -->
</Project>
```

**优势**：
- 避免各 `.csproj` 重复配置
- 统一目标框架和语言版本
- 便于版本管理

### 2. 单例硬件监控 (`HardwareMonitorProvider`)

LibreHardwareMonitor 的 `Computer` 对象是重量级资源，通过单例模式共享：

```csharp
public sealed class HardwareMonitorProvider : IHardwareMonitorProvider
{
    private Computer? _computer;

    public Computer Computer { get; }

    public HardwareMonitorProvider()
    {
        _computer = new Computer { IsCpuEnabled = true, IsGpuEnabled = true };
        _computer.Open();
    }
}
```

**优势**：
- 避免创建多个 `Computer` 实例
- CPU/GPU 监控服务共享同一实例
- 统一生命周期管理

### 3. 高效事件日志读取 (`EventLogService`)

使用 `EventLogReader` + 结构化 XML 查询实现服务端过滤：

```csharp
var queryXml = $@"
  <QueryList>
    <Query Id='0' Path='{logName}'>
      <Select Path='{logName}'>
        *[System[(Level=2 or Level=3) and 
          TimeCreated[@SystemTime&gt;='{cutoffStr}']]]
      </Select>
    </Query>
  </QueryList>";

using var reader = new EventLogReader(new EventLogQuery(logName, PathType.LogName, queryXml));
```

**优势**：
- 服务端过滤，减少数据传输
- 替代遍历全部条目，大幅提升性能
- 支持 UTC 时间范围查询

### 4. CancellationToken 全链路支持

所有耗时操作均支持取消：

```csharp
public interface IHardwareService
{
    HardwareData GetHardwareInfo(CancellationToken cancellationToken = default);
}

public interface IDiagnosticCollectorService
{
    Task<DiagnosticCollectionResult> CollectAsync(
        int daysBack,
        IProgress<DiagnosticCollectionProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
```

**优势**：
- 保证 UI 响应性
- 支持用户取消长时间操作
- 避免资源泄漏

### 5. 模型拆分设计

数据模型按职责拆分为独立文件：

| 文件 | 内容 |
|------|------|
| `DiagnosticData.cs` | 诊断数据总览 |
| `HardwareData.cs` | 硬件信息 |
| `DiskModels.cs` | 磁盘与 SMART 数据 |
| `ComputeModels.cs` | CPU/GPU 实时数据 |
| `EventModels.cs` | 事件日志与可靠性记录 |
| `DeviceModels.cs` | 网络/USB 设备信息 |
| `PerformanceAnalysisData.cs` | 性能分析结果 |
| `CollectionModels.cs` | 采集进度与结果 |

**优势**：
- 单一职责原则
- 便于维护和扩展
- 清晰的数据边界

### 6. 缓冲日志服务 (`FileLogService`)

使用 `StreamWriter` 替代逐次 `File.AppendAllText`：

```csharp
private StreamWriter _writer;

private void Write(string level, string message, Exception? exception)
{
    lock (_lock)
    {
        CheckLogRotation();  // 按日期和大小轮转
        _writer.WriteLine($"{timestamp} [{level}] {message}");
    }
}
```

**优势**：
- 减少 I/O 开销
- 支持日志轮转（按日期 + 大小）
- 自动清理旧日志（保留 7 天）

## 依赖注入配置

应用启动时配置所有服务：

```csharp
private static void ConfigureServices(IServiceCollection services)
{
    // UI
    services.AddSingleton<MainWindow>();
    services.AddSingleton<MainViewModel>();

    // Core Services
    services.AddSingleton<ILogService, FileLogService>();
    services.AddSingleton<IReportService, ReportService>();
    services.AddSingleton<IDiagnosticCollectorService, DiagnosticCollectorService>();

    // Hardware Monitoring
    services.AddSingleton<IHardwareMonitorProvider, HardwareMonitorProvider>();
    services.AddSingleton<ICpuMonitorService, CpuMonitorService>();
    services.AddSingleton<IGpuMonitorService, GpuMonitorService>();
    services.AddSingleton<INetworkMonitorService, NetworkMonitorService>();
    services.AddSingleton<IDiskSmartService, DiskSmartService>();
    services.AddSingleton<IHardwareService, HardwareService>();

    // Analysis
    services.AddSingleton<IReliabilityService, ReliabilityService>();
    services.AddSingleton<IEventLogService, EventLogService>();
    services.AddSingleton<ISystemInfoProvider, WmiSystemInfoProvider>();
    services.AddSingleton<IPerformanceService, PerformanceService>();
}
```

## 异常处理策略

自定义异常类型提供丰富的上下文信息：

| 异常类型 | 用途 | 特殊属性 |
|---------|------|----------|
| `ServiceException` | 服务层错误 | `ErrorType`, `ServiceName`, `FailedServices` |
| `ReportException` | 报告生成错误 | `ErrorType`, `Path`, `MissingField` |
| `WmiException` | WMI 查询错误 | `ErrorType`, `Resource`, `Query` |

所有异常都提供工厂方法便于创建：

```csharp
throw ServiceException.CollectionFailed("HardwareService", "WMI timeout");
throw ReportException.InvalidData("JSON content is empty");
throw WmiException.AccessDenied("Win32_Processor");
```
