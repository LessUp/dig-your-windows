# 数据 Schema

DigYourWindows 的诊断数据遵循 JSON Schema 规范。完整定义见 [`diagnostic-data-schema.json`](https://github.com/LessUp/dig-your-windows/blob/main/docs/diagnostic-data-schema.json)。

## 顶层结构

```json
{
  "hardware": { ... },
  "reliability": [ ... ],
  "events": [ ... ],
  "performance": { ... },
  "collectedAt": "2025-01-01T00:00:00Z"
}
```

| 字段 | 类型 | 必需 | 说明 |
|------|------|------|------|
| `hardware` | `HardwareData` | ✅ | 硬件信息 |
| `reliability` | `ReliabilityRecord[]` | ✅ | 可靠性记录 |
| `events` | `LogEvent[]` | ✅ | 事件日志 |
| `performance` | `PerformanceAnalysis` | ✅ | 性能分析 |
| `collectedAt` | `DateTime` | ✅ | 采集时间（UTC） |

## 硬件数据 (`HardwareData`)

```json
{
  "computerName": "DESKTOP-ABC",
  "osVersion": "Microsoft Windows NT 10.0.19041.0",
  "cpuBrand": "Intel(R) Core(TM) i7-12700K",
  "cpuCores": 12,
  "totalMemory": 34359738368,
  "disks": [ ... ],
  "diskSmart": [ ... ],
  "networkAdapters": [ ... ],
  "usbDevices": [ ... ],
  "usbControllers": [ ... ],
  "gpus": [ ... ]
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `computerName` | `string` | 计算机名 |
| `osVersion` | `string` | 操作系统版本 |
| `cpuBrand` | `string` | CPU 品牌/型号 |
| `cpuCores` | `uint` | CPU 核心数 |
| `totalMemory` | `ulong` | 总内存（字节） |
| `disks` | `DiskInfoData[]` | 磁盘信息 |
| `diskSmart` | `DiskSmartData[]` | 磁盘 SMART 数据 |
| `networkAdapters` | `NetworkAdapterData[]` | 网络适配器 |
| `usbDevices` | `UsbDeviceData[]` | USB 设备 |
| `usbControllers` | `UsbControllerData[]` | USB 控制器 |
| `gpus` | `GpuInfoData[]` | GPU 信息 |

### 磁盘信息 (`DiskInfoData`)

```json
{
  "name": "C:",
  "fileSystem": "NTFS",
  "totalSpace": 512110190592,
  "availableSpace": 256055097296
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `name` | `string` | 盘符/挂载点 |
| `fileSystem` | `string` | 文件系统类型 |
| `totalSpace` | `ulong` | 总空间（字节） |
| `availableSpace` | `ulong` | 可用空间（字节） |

### 磁盘 SMART (`DiskSmartData`)

```json
{
  "deviceId": "0",
  "friendlyName": "Samsung SSD 980 PRO 1TB",
  "serialNumber": "S5Z2NX0N123456",
  "busType": 17,
  "mediaType": 4,
  "size": 1000204886016,
  "healthStatus": 1,
  "temperature": 45,
  "wear": 2,
  "powerOnHours": 1234
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `deviceId` | `string` | 设备 ID |
| `friendlyName` | `string` | 友好名称 |
| `serialNumber` | `string?` | 序列号 |
| `busType` | `ushort?` | 总线类型代码 |
| `mediaType` | `ushort?` | 介质类型代码 |
| `size` | `ulong` | 容量（字节） |
| `healthStatus` | `ushort?` | 健康状态代码 |
| `temperature` | `ushort?` | 温度（°C） |
| `wear` | `ushort?` | 磨损度（%） |
| `powerOnHours` | `uint?` | 通电时间（小时） |

#### 枚举值

**BusType**:
| 代码 | 说明 |
|------|------|
| 11 | SATA |
| 17 | NVMe |
| 7 | USB |
| 10 | SAS |

**MediaType**:
| 代码 | 说明 |
|------|------|
| 3 | HDD |
| 4 | SSD |
| 5 | SCM |

**HealthStatus**:
| 代码 | 说明 |
|------|------|
| 1 | 健康 |
| 2 | 警告 |
| 3 | 故障 |

### GPU 信息 (`GpuInfoData`)

```json
{
  "name": "NVIDIA GeForce RTX 3080",
  "driverVersion": "560.70",
  "videoMemory": 10737418240,
  "temperature": 65.5,
  "load": 45.0,
  "memoryUsed": 2048.0,
  "memoryTotal": 10240.0,
  "coreClock": 1710.0,
  "memoryClock": 950.0,
  "fanSpeed": 35.0,
  "power": 120.5
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `name` | `string` | GPU 名称 |
| `temperature` | `float` | 核心温度（°C） |
| `load` | `float` | 核心负载（%） |
| `memoryUsed` | `float` | 已用显存（MB） |
| `memoryTotal` | `float` | 总显存（MB） |
| `coreClock` | `float` | 核心频率（MHz） |
| `power` | `float` | 功耗（W） |

## 事件日志 (`LogEvent`)

```json
{
  "timeGenerated": "2025-01-01T12:30:00Z",
  "logFile": "System",
  "sourceName": "Kernel-Power",
  "eventType": "Error",
  "eventId": 41,
  "message": "The system has rebooted without cleanly shutting down first."
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `timeGenerated` | `DateTime` | 事件时间（本地时区） |
| `logFile` | `string` | 日志文件名（System/Application） |
| `sourceName` | `string` | 事件源 |
| `eventType` | `string` | 事件类型（Error/Warning） |
| `eventId` | `uint` | 事件 ID |
| `message` | `string` | 事件消息 |

## 可靠性记录 (`ReliabilityRecord`)

```json
{
  "timestamp": "2025-01-01T10:00:00Z",
  "sourceName": "Application Error",
  "message": "Faulting application name: app.exe",
  "eventType": "Error",
  "recordType": 1
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `timestamp` | `DateTime` | 记录时间 |
| `sourceName` | `string` | 产品名称 |
| `message` | `string` | 消息内容 |
| `recordType` | `int?` | 记录类型代码 |

**RecordType**:
| 代码 | 说明 |
|------|------|
| 1 | 应用程序故障 |
| 2 | Windows 故障 |
| 3 | 其他故障 |

## 性能分析 (`PerformanceAnalysis`)

```json
{
  "systemHealthScore": 85.0,
  "stabilityScore": 90.0,
  "performanceScore": 88.0,
  "memoryUsageScore": 75.0,
  "diskHealthScore": 80.0,
  "systemUptimeDays": 15.5,
  "criticalIssuesCount": 1,
  "warningsCount": 5,
  "recommendations": [
    "磁盘 C: 剩余空间不足 (15%)，建议清理空间"
  ],
  "healthGrade": "良好",
  "healthColor": "#17a2b8"
}
```

| 字段 | 类型 | 范围 | 说明 |
|------|------|------|------|
| `systemHealthScore` | `double` | 0-100 | 系统健康评分 |
| `stabilityScore` | `double` | 0-100 | 稳定性评分 |
| `performanceScore` | `double` | 0-100 | 性能评分 |
| `memoryUsageScore` | `double` | 0-100 | 内存评分 |
| `diskHealthScore` | `double` | 0-100 | 磁盘健康评分 |
| `systemUptimeDays` | `double?` | - | 系统运行天数 |
| `criticalIssuesCount` | `uint` | - | 关键问题数 |
| `warningsCount` | `uint` | - | 警告数 |
| `recommendations` | `string[]` | - | 优化建议 |
| `healthGrade` | `string` | - | 健康等级 |
| `healthColor` | `string` | - | 显示颜色（Hex） |

## 健康评分计算

### 评分维度

| 维度 | 权重 | 计算依据 |
|------|------|----------|
| 稳定性 | 40% | 错误事件数、警告数、关键事件、可靠性记录 |
| 性能 | 30% | CPU 核心数、CPU 品牌、内存容量 |
| 内存 | 15% | 总内存容量 |
| 磁盘 | 15% | 磁盘剩余空间比例 |

### 健康等级

| 等级 | 分数范围 | 颜色 | 说明 |
|------|----------|------|------|
| 优秀 | 90-100 | `#28a745` | 系统状态良好 |
| 良好 | 75-89 | `#17a2b8` | 系统状态正常 |
| 一般 | 60-74 | `#ffc107` | 存在一些问题 |
| 较差 | 40-59 | `#fd7e14` | 需要关注 |
| 需要优化 | < 40 | `#dc3545` | 需要立即处理 |
