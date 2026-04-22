# Hardware Specification

> **Domain**: hardware
> **Version**: 1.0.0
> **Status**: accepted
> **Last Updated**: 2026-04-23

## Overview

硬件监控领域规范，定义 CPU、GPU、内存、磁盘和网络监控的行为契约。

## Requirements

### Requirement: CPU Monitoring

系统 SHALL 提供实时 CPU 温度、频率和负载数据。

#### Scenario: 有效 CPU 数据获取
- GIVEN 系统具有可访问的 CPU 传感器
- WHEN 调用 CpuMonitorService.GetCpuData()
- THEN 返回温度数据（摄氏度，-40 到 150°C）
- AND 返回负载百分比（0-100%）
- AND 返回频率数据（GHz）

#### Scenario: CPU 传感器不可用
- GIVEN CPU 传感器不可访问（权限不足或硬件不支持）
- WHEN 调用 CpuMonitorService.GetCpuData()
- THEN 返回默认值或 null
- AND 不抛出异常

### Requirement: GPU Monitoring

系统 SHALL 提供 GPU 温度、负载和显存使用数据。

#### Scenario: NVIDIA GPU 数据获取
- GIVEN 系统安装 NVIDIA 显卡
- WHEN 调用 GpuMonitorService.GetGpuData()
- THEN 返回温度数据（0-120°C）
- AND 返回负载百分比（0-100%）
- AND 返回显存使用量（bytes）

#### Scenario: AMD GPU 数据获取
- GIVEN 系统安装 AMD 显卡
- WHEN 调用 GpuMonitorService.GetGpuData()
- THEN 返回与 NVIDIA 相同格式的数据

#### Scenario: 无独立显卡
- GIVEN 系统无独立显卡或集显不支持监控
- WHEN 调用 GpuMonitorService.GetGpuData()
- THEN 返回 null 或默认值

### Requirement: Disk SMART Monitoring

系统 SHALL 从 NVMe 和 SATA 存储设备读取 SMART 属性。

#### Scenario: NVMe 磁盘健康状态
- GIVEN NVMe 固态硬盘
- WHEN 调用 DiskSmartService.GetDiskSmartData()
- THEN 返回健康状态（Good/Fair/Poor/Unknown）
- AND 返回温度数据
- AND 返回 SMART 属性列表

#### Scenario: SATA 磁盘 SMART 数据
- GIVEN SATA 接口磁盘
- WHEN 调用 DiskSmartService.GetDiskSmartData()
- THEN 返回 SMART 属性数据

#### Scenario: 管理员权限不足
- GIVEN 程序未以管理员身份运行
- WHEN 尝试读取 SMART 数据
- THEN 返回健康状态 Unknown
- AND 记录警告日志

### Requirement: Network Monitoring

系统 SHALL 监控网络适配器带宽和状态。

#### Scenario: 网络适配器状态
- GIVEN 存在活动的网络适配器
- WHEN 调用 NetworkMonitorService.GetNetworkData()
- THEN 返回适配器名称列表
- AND 返回每个适配器的带宽使用情况

### Requirement: Memory Monitoring

系统 SHALL 提供内存使用情况数据。

#### Scenario: 内存使用数据
- WHEN 调用 HardwareService 获取内存数据
- THEN 返回总内存（bytes）
- AND 返回可用内存（bytes）
- AND 返回使用百分比（0-100%）

## Hardware Abstraction Layer

### IHardwareMonitorProvider

`HardwareMonitorProvider` 单例提供对 LibreHardwareMonitor `Computer` 对象的访问。

```csharp
public interface IHardwareMonitorProvider
{
    Computer Computer { get; }
}
```

**设计模式**：线程安全的延迟初始化单例

**生命周期**：应用启动时创建，退出时释放

## Thresholds

| 指标 | 正常 | 警告 | 危险 |
|------|------|------|------|
| CPU 温度 | <70°C | 70-85°C | >85°C |
| GPU 温度 | <75°C | 75-90°C | >90°C |
| 磁盘温度 | <50°C | 50-65°C | >65°C |
| 内存使用率 | <70% | 70-90% | >90% |
| 磁盘健康状态 | Good | Fair | Poor |

## Error Handling

### 优雅降级策略

| 场景 | 行为 |
|------|------|
| 管理员权限不足 | 返回默认值，记录警告，UI 显示受限状态 |
| LibreHardwareMonitor 初始化失败 | 禁用 GPU/温度监控，保留 WMI 数据 |
| 传感器不可用 | 返回 null，不抛出异常 |

## Performance Requirements

| 操作 | 目标性能 |
|------|---------|
| 硬件初始化 | < 2000ms |
| 实时数据更新 | < 100ms |
| 内存占用 | < 50MB 额外开销 |

## References

- [Architecture Specification](../architecture/spec.md)
- [Data Specification](../data/spec.md)
