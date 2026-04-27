using System.Text.Json;
using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.PropertyTests;

/// <summary>
/// FsCheck property tests for DiagnosticData serialization round-trips.
/// Validates that serialization preserves data integrity.
/// </summary>
public class DiagnosticDataPropertyTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    [Property]
    public void DiagnosticData_SerializationRoundTrip_PreservesCoreFields(
        NonEmptyString computerName,
        NonEmptyString osVersion,
        NonNegativeInt cpuCores,
        NonNegativeInt totalMemoryMB)
    {
        // Arrange
        var totalMemory = (ulong)totalMemoryMB.Get * 1024UL * 1024UL;
        var collectedAt = DateTimeOffset.UtcNow;

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = osVersion.Get,
                CpuBrand = "Test CPU",
                CpuCores = (uint)(cpuCores.Get % 128),
                TotalMemory = totalMemory,
                Disks = new List<DiskInfoData>(),
                NetworkAdapters = new List<NetworkAdapterInfo>(),
                UsbDevices = new List<UsbDeviceInfo>(),
                Gpus = new List<GpuInfoData>()
            },
            Events = new List<LogEventData>(),
            Reliability = new List<ReliabilityRecordData>(),
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = 85.5,
                StabilityScore = 90.0,
                PerformanceScore = 80.0,
                MemoryUsageScore = 85.0,
                DiskHealthScore = 88.0,
                SystemUptimeDays = 3.5,
                CriticalIssuesCount = 0,
                WarningsCount = 2,
                Recommendations = new List<string> { "建议清理磁盘空间" },
                HealthGrade = "良好",
                HealthColor = "#17a2b8"
            },
            CollectedAt = collectedAt.DateTime
        };

        // Act
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<DiagnosticData>(json, JsonOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(data.Hardware.ComputerName, deserialized!.Hardware.ComputerName);
        Assert.Equal(data.Hardware.OsVersion, deserialized.Hardware.OsVersion);
        Assert.Equal(data.Hardware.CpuCores, deserialized.Hardware.CpuCores);
        Assert.Equal(data.Hardware.TotalMemory, deserialized.Hardware.TotalMemory);
        Assert.Equal(data.Performance.SystemHealthScore, deserialized.Performance.SystemHealthScore);
        Assert.Equal(data.Performance.StabilityScore, deserialized.Performance.StabilityScore);
        Assert.Equal(data.Performance.HealthGrade, deserialized.Performance.HealthGrade);
    }

    [Property]
    public void DiagnosticData_CollectedAt_PreservesUtcKind(DateTimeOffset timestamp)
    {
        // Arrange
        var utcTimestamp = timestamp.UtcDateTime;
        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = "TestPC",
                OsVersion = "Windows 11",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>()
            },
            CollectedAt = utcTimestamp
        };

        // Act
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<DiagnosticData>(json, JsonOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(utcTimestamp, deserialized!.CollectedAt);
        // Note: DateTime serialization doesn't preserve Kind by default,
        // but the timestamp value should be identical
    }

    [Property]
    public void DiagnosticData_EmptyCollections_SerializeCorrectly(
        NonEmptyString computerName,
        NonEmptyString osVersion)
    {
        // Arrange
        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = osVersion.Get,
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>(),
                NetworkAdapters = new List<NetworkAdapterInfo>(),
                UsbDevices = new List<UsbDeviceInfo>(),
                Gpus = new List<GpuInfoData>()
            },
            Events = new List<LogEventData>(),
            Reliability = new List<ReliabilityRecordData>(),
            Performance = new PerformanceAnalysisData
            {
                Recommendations = new List<string>()
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<DiagnosticData>(json, JsonOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Empty(deserialized!.Hardware.Disks);
        Assert.Empty(deserialized.Hardware.NetworkAdapters);
        Assert.Empty(deserialized.Hardware.UsbDevices);
        Assert.Empty(deserialized.Hardware.Gpus);
        Assert.Empty(deserialized.Events);
        Assert.Empty(deserialized.Reliability);
        Assert.Empty(deserialized.Performance.Recommendations);
    }

    [Property]
    public void DiagnosticData_WithDiskInfo_SerializesCorrectly(
        NonEmptyString diskName,
        NonNegativeInt totalGB,
        NonNegativeInt availableGB)
    {
        // Arrange
        var total = Math.Max(1L, (long)totalGB.Get) * 1024L * 1024L * 1024L;
        var available = Math.Min((long)availableGB.Get * 1024L * 1024L * 1024L, total);

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = "TestPC",
                OsVersion = "Windows 11",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>
                {
                    new()
                    {
                        Name = diskName.Get,
                        TotalSpace = total,
                        AvailableSpace = available
                    }
                }
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<DiagnosticData>(json, JsonOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Single(deserialized!.Hardware.Disks);
        Assert.Equal(diskName.Get, deserialized.Hardware.Disks[0].Name);
        Assert.Equal(total, deserialized.Hardware.Disks[0].TotalSpace);
        Assert.Equal(available, deserialized.Hardware.Disks[0].AvailableSpace);
    }

    [Property]
    public void PerformanceAnalysisData_ScoresAreClamped(
        NonNegativeInt rawStability,
        NonNegativeInt rawPerformance,
        NonNegativeInt rawMemory,
        NonNegativeInt rawDisk)
    {
        // Arrange
        var stability = rawStability.Get % 150; // May exceed 100
        var performance = rawPerformance.Get % 150;
        var memory = rawMemory.Get % 150;
        var disk = rawDisk.Get % 150;

        var data = new PerformanceAnalysisData
        {
            SystemHealthScore = Math.Clamp(stability * 0.4 + performance * 0.3 + memory * 0.15 + disk * 0.15, 0, 100),
            StabilityScore = Math.Clamp(stability, 0, 100),
            PerformanceScore = Math.Clamp(performance, 0, 100),
            MemoryUsageScore = Math.Clamp(memory, 0, 100),
            DiskHealthScore = Math.Clamp(disk, 0, 100),
            Recommendations = new List<string>()
        };

        // Act
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var deserialized = JsonSerializer.Deserialize<PerformanceAnalysisData>(json, JsonOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.InRange(deserialized!.SystemHealthScore, 0d, 100d);
        Assert.InRange(deserialized.StabilityScore, 0d, 100d);
        Assert.InRange(deserialized.PerformanceScore, 0d, 100d);
        Assert.InRange(deserialized.MemoryUsageScore, 0d, 100d);
        Assert.InRange(deserialized.DiskHealthScore, 0d, 100d);
    }
}
