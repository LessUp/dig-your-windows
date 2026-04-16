# Data Schema

DigYourWindows diagnostic data follows JSON Schema specification. This document details the data model structures.

## Quick Reference

### Root Diagnostic Object

```json
{
  "hardware": { },      // Hardware information
  "reliability": [ ],   // Reliability records array
  "events": [ ],        // Event log array
  "performance": { },   // Performance analysis
  "collectedAt": "2025-01-01T00:00:00Z"
}
```

### Field Summary

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `hardware` | `HardwareData` | ✅ | Hardware information |
| `reliability` | `ReliabilityRecord[]` | ✅ | Reliability records |
| `events` | `LogEvent[]` | ✅ | Event logs |
| `performance` | `PerformanceAnalysis` | ✅ | Performance analysis |
| `collectedAt` | `DateTime` | ✅ | Collection time (UTC) |

## Hardware Data (`HardwareData`)

### Hardware Fields

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `computerName` | `string` | Computer name | `"DESKTOP-ABC"` |
| `osVersion` | `string` | OS version | `"Windows NT 10.0.22621"` |
| `cpuBrand` | `string` | CPU model | `"Intel Core i7-12700K"` |
| `cpuCores` | `uint` | CPU core count | `12` |
| `totalMemory` | `ulong` | Total RAM (bytes) | `34359738368` |
| `disks` | `DiskInfoData[]` | Disk partitions | See below |
| `diskSmart` | `DiskSmartData[]` | SMART data | See below |
| `networkAdapters` | `NetworkAdapterData[]` | Network adapters | See below |
| `gpus` | `GpuInfoData[]` | GPU information | See below |

### SMART Data (`DiskSmartData`)

```json
{
  "deviceId": "0",
  "friendlyName": "Samsung SSD 980 PRO",
  "serialNumber": "S5Z2NX0N123456",
  "busType": 17,      // 11=SATA, 17=NVMe
  "mediaType": 4,     // 3=HDD, 4=SSD
  "size": 1000204886016,
  "healthStatus": 1,  // 1=Healthy, 2=Warning, 3=Failed
  "temperature": 45,
  "wear": 2,          // Wear percentage
  "powerOnHours": 1234
}
```

### GPU Info (`GpuInfoData`)

```json
{
  "name": "NVIDIA GeForce RTX 3080",
  "temperature": 65.5,      // °C
  "load": 45.0,             // %
  "memoryUsed": 2048.0,     // MB
  "memoryTotal": 10240.0,   // MB
  "coreClock": 1710.0,      // MHz
  "power": 120.5            // Watts
}
```

## Event Log (`LogEvent`)

```json
{
  "timeGenerated": "2025-01-01T12:30:00",
  "logFile": "System",
  "sourceName": "Kernel-Power",
  "eventType": "Error",   // Error or Warning
  "eventId": 41,
  "message": "System rebooted without clean shutdown"
}
```

## Performance Analysis (`PerformanceAnalysis`)

```json
{
  "systemHealthScore": 85.0,    // 0-100 overall score
  "stabilityScore": 90.0,       // 40% weight
  "performanceScore": 88.0,     // 30% weight
  "memoryUsageScore": 75.0,     // 15% weight
  "diskHealthScore": 80.0,      // 15% weight
  "criticalIssuesCount": 1,
  "warningsCount": 5,
  "recommendations": [
    "Disk C: low space (15%), consider cleanup"
  ],
  "healthGrade": "Good",
  "healthColor": "#17a2b8"
}
```

### Health Grade Scale

| Score | Grade | Color | Status |
|-------|-------|-------|--------|
| 90-100 | Excellent | `#28a745` 🟢 | Optimal |
| 75-89 | Good | `#17a2b8` 🔵 | Normal |
| 60-74 | Fair | `#ffc107` 🟡 | Some issues |
| 40-59 | Poor | `#fd7e14` 🟠 | Attention needed |
| < 40 | Critical | `#dc3545` 🔴 | Immediate action |

### Scoring Algorithm

```csharp
// Weight distribution
var totalScore = 
    stabilityScore * 0.40 +
    performanceScore * 0.30 +
    memoryScore * 0.15 +
    diskScore * 0.15;
```

## Schema File

Complete JSON Schema definition: [`diagnostic-data-schema.json`](https://github.com/LessUp/dig-your-windows/blob/main/docs/diagnostic-data-schema.json)

## Version Compatibility

| Version | Schema | Compatibility |
|---------|--------|---------------|
| 1.0.x | v1.0 | Backward compatible with 0.5.x |
| 0.5.x | v0.5 | Not compatible with 0.4.x |
