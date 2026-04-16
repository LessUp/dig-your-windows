# 数据 Schema

DigYourWindows 的诊断数据遵循 JSON Schema 规范。本文档详细说明数据模型的结构和用途。

## 快速参考

### 诊断数据根对象

```json
{
  "hardware": { ... },      // 硬件信息
  "reliability": [ ... ],   // 可靠性记录数组
  "events": [ ... ],        // 事件日志数组
  "performance": { ... },   // 性能分析结果
  "collectedAt": "2025-01-01T00:00:00Z"  // 采集时间（UTC）
}
```

### 字段摘要

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
  "computerName": "DESKTOP-ABC123",
  "osVersion": "Microsoft Windows NT 10.0.22621.0",
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

### 字段详情

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| `computerName` | `string` | 计算机名称 | `"DESKTOP-ABC123"` |
| `osVersion` | `string` | 操作系统版本 | `"Microsoft Windows NT 10.0.22621.0"` |
| `cpuBrand` | `string` | CPU 品牌型号 | `"Intel Core i7-12700K"` |
| `cpuCores` | `uint` | CPU 核心数 | `12` |
| `totalMemory` | `ulong` | 总内存（字节）| `34359738368` (32GB) |
| `disks` | `DiskInfoData[]` | 磁盘分区信息 | 见下文 |
| `diskSmart` | `DiskSmartData[]` | 磁盘 SMART 数据 | 见下文 |
| `networkAdapters` | `NetworkAdapterData[]` | 网络适配器 | 见下文 |
| `usbDevices` | `UsbDeviceData[]` | USB 设备 | 见下文 |
| `usbControllers` | `UsbControllerData[]` | USB 控制器 | 见下文 |
| `gpus` | `GpuInfoData[]` | GPU 信息 | 见下文 |

### 磁盘信息 (`DiskInfoData`)

```json
{
  "name": "C:",
  "fileSystem": "NTFS",
  "totalSpace": 512110190592,
  "availableSpace": 256055097296
}
```

| 字段 | 类型 | 说明 | 计算方式 |
|------|------|------|----------|
| `name` | `string` | 盘符/挂载点 | - |
| `fileSystem` | `string` | 文件系统类型 | NTFS/FAT32/exFAT |
| `totalSpace` | `ulong` | 总空间（字节）| - |
| `availableSpace` | `ulong` | 可用空间（字节）| - |

**使用示例**：计算使用百分比
```csharp
var usagePercent = (1 - (double)disk.AvailableSpace / disk.TotalSpace) * 100;
```

### 磁盘 SMART (`DiskSmartData`)

```json
{
  "deviceId": "0",
  "friendlyName": "Samsung SSD 980 PRO 1TB",
  "serialNumber": "S5Z2NX0N123456X",
  "busType": 17,
  "mediaType": 4,
  "size": 1000204886016,
  "healthStatus": 1,
  "temperature": 45,
  "wear": 2,
  "powerOnHours": 1234
}
```

| 字段 | 类型 | 说明 | 备注 |
|------|------|------|------|
| `deviceId` | `string` | 设备 ID | 物理设备标识 |
| `friendlyName` | `string` | 友好名称 | 厂商型号 |
| `serialNumber` | `string?` | 序列号 | 可能为空 |
| `busType` | `ushort?` | 总线类型代码 | 见枚举表 |
| `mediaType` | `ushort?` | 介质类型代码 | 见枚举表 |
| `size` | `ulong` | 容量（字节）| - |
| `healthStatus` | `ushort?` | 健康状态代码 | 见枚举表 |
| `temperature` | `ushort?` | 温度（°C）| 需要管理员权限 |
| `wear` | `ushort?` | 磨损度（%）| SSD 剩余寿命 |
| `powerOnHours` | `uint?` | 通电时间（小时）| - |

#### 枚举值对照表

**BusType（总线类型）**:
| 代码 | 说明 |
|------|------|
| `7` | USB |
| `10` | SAS |
| `11` | SATA |
| `17` | NVMe |

**MediaType（介质类型）**:
| 代码 | 说明 |
|------|------|
| `3` | HDD（机械硬盘）|
| `4` | SSD（固态硬盘）|
| `5` | SCM（存储级内存）|

**HealthStatus（健康状态）**:
| 代码 | 说明 | 建议操作 |
|------|------|----------|
| `1` | 健康 | 正常使用 |
| `2` | 警告 | 建议备份数据 |
| `3` | 故障 | 立即更换 |

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
  "memoryClock": 1900.0,
  "fanSpeed": 35.0,
  "power": 120.5
}
```

| 字段 | 类型 | 说明 | 单位 |
|------|------|------|------|
| `name` | `string` | GPU 名称 | - |
| `driverVersion` | `string` | 驱动版本 | - |
| `videoMemory` | `long` | 显存容量（字节）| bytes |
| `temperature` | `float` | 核心温度 | °C |
| `load` | `float` | 核心负载 | % |
| `memoryUsed` | `float` | 已用显存 | MB |
| `memoryTotal` | `float` | 总显存 | MB |
| `coreClock` | `float` | 核心频率 | MHz |
| `memoryClock` | `float` | 显存频率 | MHz |
| `fanSpeed` | `float` | 风扇转速 | % |
| `power` | `float` | 功耗 | W |

**注意**：温度、负载等实时数据需要管理员权限且依赖 LIBreHardwareMonitor 支持。

### 网络适配器 (`NetworkAdapterData`)

```json
{
  "name": "Intel(R) Wi-Fi 6E AX210",
  "macAddress": "AA:BB:CC:DD:EE:FF",
  "ipAddresses": ["192.168.1.100", "fe80::1234"],
  "isEnabled": true,
  "speedMbps": 2402
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `name` | `string` | 适配器名称 |
| `macAddress` | `string` | MAC 地址 |
| `ipAddresses` | `string[]` | IP 地址列表（IPv4 + IPv6）|
| `isEnabled` | `bool` | 是否启用 |
| `speedMbps` | `long` | 链接速度（Mbps）|

### USB 设备 (`UsbDeviceData`)

```json
{
  "deviceId": "USB\\VID_046D&PID_C52B",
  "description": "USB Receiver",
  "manufacturer": "Logitech",
  "vendorId": "046D",
  "productId": "C52B"
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `deviceId` | `string` | 设备 ID |
| `description` | `string` | 设备描述 |
| `manufacturer` | `string` | 制造商 |
| `vendorId` | `string` | 厂商 ID（VID）|
| `productId` | `string` | 产品 ID（PID）|

## 事件日志 (`LogEvent`)

```json
{
  "timeGenerated": "2025-01-01T12:30:00",
  "logFile": "System",
  "sourceName": "Kernel-Power",
  "eventType": "Error",
  "eventId": 41,
  "message": "The system has rebooted without cleanly shutting down first."
}
```

| 字段 | 类型 | 说明 | 备注 |
|------|------|------|------|
| `timeGenerated` | `DateTime` | 事件时间 | 本地时区 |
| `logFile` | `string` | 源日志 | System/Application |
| `sourceName` | `string` | 事件源 | 提供者 |
| `eventType` | `string` | 事件类型 | Error/Warning |
| `eventId` | `uint` | 事件 ID | Windows 定义 |
| `message` | `string` | 事件消息 | 可能截断 |

**事件类型说明**:
- `Error`: 错误事件
- `Warning`: 警告事件

## 可靠性记录 (`ReliabilityRecord`)

```json
{
  "timestamp": "2025-01-01T10:00:00",
  "sourceName": "Application Error",
  "message": "Faulting application name: app.exe",
  "eventType": "Error",
  "recordType": 1
}
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `timestamp` | `DateTime` | 记录时间 |
| `sourceName` | `string` | 源名称 |
| `message` | `string` | 消息内容 |
| `eventType` | `string` | 事件类型（Error/Warning）|
| `recordType` | `int?` | 记录类型代码 |

**RecordType（记录类型）**:
| 代码 | 说明 |
|------|------|
| `1` | 应用程序故障 |
| `2` | Windows 故障 |
| `3` | 其他故障 |

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

### 字段详情

| 字段 | 类型 | 范围 | 说明 |
|------|------|------|------|
| `systemHealthScore` | `double` | 0-100 | 系统健康总评分 |
| `stabilityScore` | `double` | 0-100 | 稳定性评分 |
| `performanceScore` | `double` | 0-100 | 性能评分 |
| `memoryUsageScore` | `double` | 0-100 | 内存评分 |
| `diskHealthScore` | `double` | 0-100 | 磁盘健康评分 |
| `systemUptimeDays` | `double?` | - | 系统运行天数 |
| `criticalIssuesCount` | `uint` | - | 关键问题数 |
| `warningsCount` | `uint` | - | 警告数 |
| `recommendations` | `string[]` | - | 优化建议列表 |
| `healthGrade` | `string` | - | 健康等级 |
| `healthColor` | `string` | - | 显示颜色（Hex）|

### 健康评分计算

#### 评分维度与权重

| 维度 | 权重 | 计算依据 |
|------|------|----------|
| **稳定性** | 40% | 错误事件数、警告数、关键事件、可靠性记录 |
| **性能** | 30% | CPU 核心数、CPU 品牌、内存容量 |
| **内存** | 15% | 总内存容量 |
| **磁盘** | 15% | 磁盘剩余空间比例、SMART 健康状态 |

#### 计算公式

```csharp
// 1. 基础分数计算
var stabilityScore = CalculateStabilityScore(events, reliabilityRecords);
var performanceScore = CalculatePerformanceScore(cpuCores, totalMemory);
var memoryScore = CalculateMemoryScore(totalMemory);
var diskScore = CalculateDiskScore(disks, smartData);

// 2. 加权平均
var totalScore = 
    stabilityScore * 0.40 +
    performanceScore * 0.30 +
    memoryScore * 0.15 +
    diskScore * 0.15;

// 3. 扣分项
var finalScore = totalScore - PenaltyForCriticalIssues(criticalCount);
```

#### 健康等级

| 分数范围 | 等级 | 颜色 | 说明 | 建议 |
|----------|------|------|------|------|
| 90-100 | 优秀 | `#28a745` 🟢 | 系统状态极佳 | 保持现状 |
| 75-89 | 良好 | `#17a2b8` 🔵 | 系统状态正常 | 定期维护 |
| 60-74 | 一般 | `#ffc107` 🟡 | 存在一些问题 | 注意监控 |
| 40-59 | 较差 | `#fd7e14` 🟠 | 需要关注 | 建议优化 |
| < 40 | 需要优化 | `#dc3545` 🔴 | 需要立即处理 | 紧急修复 |

## JSON Schema 文件

完整的 JSON Schema 定义见 [`diagnostic-data-schema.json`](https://github.com/LessUp/dig-your-windows/blob/main/docs/diagnostic-data-schema.json)。

Schema 可用于：
1. **数据验证**: 在导入前验证 JSON 文件
2. **代码生成**: 自动生成数据模型类
3. **文档生成**: 自动生成 API 文档

## 版本兼容性

| 版本 | Schema 版本 | 兼容性说明 |
|------|-------------|------------|
| 1.0.x | v1.0 | 向后兼容 0.5.x |
| 0.5.x | v0.5 | 不兼容 0.4.x |
| 0.4.x | v0.4 | 初始稳定版本 |

**迁移说明**:
- v0.5 到 v1.0: `GpuData` 重命名为 `GpuInfoData`，字段增加
- v0.4 到 v0.5: `DiskSmartData` 结构大幅调整
