# Data Specification

> **Domain**: data
> **Version**: 1.1.0
> **Status**: accepted
> **Last Updated**: 2026-04-27

## Overview

本文档定义 DigYourWindows 用于诊断数据收集、存储和导出的数据模型和规范。

## Root Data Structure

### DiagnosticData

表示完整诊断报告的根对象。

```json
{
  "hardware": {
    "computerName": "DESKTOP-ABC123",
    "osVersion": "Windows 11 Pro 23H2",
    "cpuBrand": "Intel Core i7-12700K",
    "cpuCores": 12,
    "totalMemory": 34359738368,
    "disks": [...],
    "diskSmart": [...],
    "networkAdapters": [...],
    "usbDevices": [...],
    "gpus": [...]
  },
  "events": [...],
  "reliability": [...],
  "performance": {
    "systemHealthScore": 85.5,
    "stabilityScore": 90.0,
    "performanceScore": 80.0,
    "memoryUsageScore": 85.0,
    "diskHealthScore": 88.0,
    "systemUptimeDays": 3.5,
    "criticalIssuesCount": 0,
    "warningsCount": 2,
    "recommendations": ["建议清理磁盘空间"],
    "healthGrade": "良好",
    "healthColor": "#17a2b8"
  },
  "collectedAt": "2026-04-27T10:30:00Z"
}
```

**字段**：

| 字段 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `hardware` | object | 是 | 硬件信息 |
| `events` | array | 是 | 事件日志列表 |
| `reliability` | array | 是 | 可靠性监视器记录 |
| `performance` | object | 是 | 性能分析结果 |
| `collectedAt` | string (ISO 8601) | 是 | 收集时间戳（UTC） |

## System Information

```json
{
  "computerName": "DESKTOP-ABC123",
  "osVersion": "Windows 11 Pro 23H2",
  "osBuild": "22631.3447",
  "architecture": "x64",
  "processor": "Intel Core i7-12700K",
  "installedMemory": "32 GB",
  "collectionDate": "2026-04-17T10:30:00Z"
}
```

**字段**：

| 字段 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `computerName` | string | 是 | 计算机名 |
| `osVersion` | string | 是 | Windows 版本 |
| `osBuild` | string | 是 | OS 构建号 |
| `architecture` | string | 是 | CPU 架构 |
| `processor` | string | 是 | 处理器型号 |
| `installedMemory` | string | 是 | 总内存 |
| `collectionDate` | string (ISO 8601) | 是 | 收集日期 |

## Hardware Data

### CPU Data

```json
{
  "name": "Intel Core i7-12700K",
  "cores": 12,
  "threads": 20,
  "baseFrequency": 3.6,
  "currentFrequency": 4.2,
  "temperature": 45.5,
  "load": 25.3
}
```

**字段**：

| 字段 | 类型 | 必需 | 单位 | 描述 |
|------|------|------|------|------|
| `name` | string | 是 | - | CPU 型号 |
| `cores` | integer | 是 | - | 物理核心数 |
| `threads` | integer | 是 | - | 逻辑线程数 |
| `baseFrequency` | number | 是 | GHz | 基础频率 |
| `currentFrequency` | number | 是 | GHz | 当前频率 |
| `temperature` | number | 是 | °C | 当前温度 |
| `load` | number | 是 | % | 当前负载 |

**验证规则**：
- 核心数必须是正整数
- 频率必须在 0.1 到 10.0 GHz 之间
- 温度必须在 -40 到 150 °C 之间
- 负载必须在 0 到 100% 之间

### Memory Data

```json
{
  "total": 34359738368,
  "available": 21474836480,
  "used": 12884901888,
  "usagePercent": 37.5,
  "speed": 3200,
  "type": "DDR4"
}
```

**字段**：

| 字段 | 类型 | 必需 | 单位 | 描述 |
|------|------|------|------|------|
| `total` | integer | 是 | bytes | 总物理内存 |
| `available` | integer | 是 | bytes | 可用内存 |
| `used` | integer | 是 | bytes | 已用内存 |
| `usagePercent` | number | 是 | % | 内存使用率 |
| `speed` | integer | 否 | MHz | 内存速度 |
| `type` | string | 否 | - | 内存类型 (DDR4/DDR5) |

**验证规则**：
- 总内存必须是正数
- 可用内存 <= 总内存
- 使用率必须在 0-100% 之间

### Disk Data

```json
{
  "model": "Samsung SSD 980 PRO 1TB",
  "serialNumber": "S5EWNF0R******",
  "size": 1000204886016,
  "interface": "NVMe",
  "temperature": 42,
  "healthStatus": "Good"
}
```

**字段**：

| 字段 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `model` | string | 是 | 磁盘型号 |
| `serialNumber` | string | 是 | 序列号（导出时脱敏） |
| `size` | integer | 是 | 磁盘大小（bytes） |
| `interface` | string | 是 | 接口类型 (NVMe/SATA) |
| `temperature` | number | 否 | 当前温度 (°C) |
| `healthStatus` | string | 是 | 健康状态 (Good/Fair/Poor/Unknown) |

**验证规则**：
- 磁盘大小必须是正数
- 温度必须在 0 到 100 °C 之间（如果可用）
- 健康状态必须是有效的枚举值

### GPU Data

```json
{
  "name": "NVIDIA GeForce RTX 3080",
  "temperature": 65,
  "load": 45.2,
  "memoryUsed": 4294967296,
  "memoryTotal": 10737418240,
  "memoryUsagePercent": 40
}
```

**字段**：

| 字段 | 类型 | 必需 | 单位 | 描述 |
|------|------|------|------|------|
| `name` | string | 是 | - | GPU 型号 |
| `temperature` | number | 是 | °C | 当前温度 |
| `load` | number | 是 | % | 当前负载 |
| `memoryUsed` | integer | 是 | bytes | 已用显存 |
| `memoryTotal` | integer | 是 | bytes | 总显存 |
| `memoryUsagePercent` | number | 是 | % | 显存使用率 |

**验证规则**：
- 温度必须在 0 到 120 °C 之间
- 负载必须在 0 到 100% 之间
- 已用显存 <= 总显存

## Event Log Data

### LogEvent

```json
{
  "timeCreated": "2026-04-17T08:15:30Z",
  "level": "Error",
  "source": "Disk",
  "eventId": 129,
  "message": "The device has a bad block.",
  "logName": "System"
}
```

**字段**：

| 字段 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `timeCreated` | string (ISO 8601) | 是 | 事件时间戳 |
| `level` | string | 是 | Error/Warning/Information |
| `source` | string | 是 | 事件来源 |
| `eventId` | integer | 是 | 事件 ID |
| `message` | string | 是 | 事件消息 |
| `logName` | string | 是 | 日志名称 (System/Application) |

## Performance Analysis

### PerformanceAnalysisData

```json
{
  "systemHealthScore": 85.5,
  "stabilityScore": 90.0,
  "performanceScore": 80.0,
  "memoryUsageScore": 85.0,
  "diskHealthScore": 88.0,
  "systemUptimeDays": 3.5,
  "criticalIssuesCount": 0,
  "warningsCount": 2,
  "recommendations": ["建议清理磁盘空间"],
  "healthGrade": "良好",
  "healthColor": "#17a2b8"
}
```

**字段**：

| 字段 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `systemHealthScore` | number | 是 | 系统健康总分 (0-100) |
| `stabilityScore` | number | 是 | 稳定性得分 (0-100) |
| `performanceScore` | number | 是 | 性能得分 (0-100) |
| `memoryUsageScore` | number | 是 | 内存使用得分 (0-100) |
| `diskHealthScore` | number | 是 | 磁盘健康得分 (0-100) |
| `systemUptimeDays` | number | 否 | 系统运行天数 |
| `criticalIssuesCount` | integer | 是 | 严重问题数量 |
| `warningsCount` | integer | 是 | 警告数量 |
| `recommendations` | array[string] | 是 | 优化建议列表 |
| `healthGrade` | string | 是 | 健康等级 (优秀/良好/一般/较差/需要优化) |
| `healthColor` | string | 是 | 健康等级颜色 (十六进制) |

**评分权重**：
- 稳定性得分权重: 40%
- 性能得分权重: 30%
- 内存得分权重: 15%
- 磁盘得分权重: 15%

## Data Validation Rules

1. **时间戳**：所有时间戳必须是 ISO 8601 格式，带 UTC 时区
2. **内存值**：所有内存值（bytes）必须是正整数
3. **温度值**：所有温度值必须是摄氏度，范围 -40 到 150
4. **百分比**：所有百分比值必须在 0 到 100 之间
5. **健康状态**：必须是以下之一："Good", "Fair", "Poor", "Unknown"
6. **优先级**：必须是以下之一："Low", "Medium", "High", "Critical"

## Security & Privacy

1. **序列号脱敏**：导出时磁盘序列号被截断
2. **MAC 地址掩码**：报告中 MAC 地址仅显示最后 2 段
3. **无云上传**：所有数据保持本地
4. **日志清洗**：日志中删除用户名和路径

## References

- [Architecture Specification](../architecture/spec.md)
- [Export Specification](../export/spec.md)
