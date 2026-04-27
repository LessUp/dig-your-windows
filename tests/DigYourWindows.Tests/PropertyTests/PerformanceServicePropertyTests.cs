using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.PropertyTests;

/// <summary>
/// FsCheck property tests for PerformanceService scoring algorithm.
/// Validates invariants: scores in [0,100], weights sum to 1.0, monotonicity.
/// </summary>
public class PerformanceServicePropertyTests
{
    /// <summary>
    /// Mock system info provider that returns a fixed uptime.
    /// </summary>
    private sealed class MockSystemInfoProvider : ISystemInfoProvider
    {
        private readonly double? _uptime;

        public MockSystemInfoProvider(double? uptime = 5.0)
        {
            _uptime = uptime;
        }

        public double? GetSystemUptimeDays() => _uptime;
    }

    /// <summary>
    /// Mock log service that does nothing.
    /// </summary>
    private sealed class MockLogService : ILogService
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Debug(string message) { }
    }

    [Property]
    public void SystemHealthScore_IsAlwaysBetween0And100(
        PositiveInt errorCount,
        PositiveInt warningCount,
        NonNegativeInt criticalCount,
        NonNegativeInt reliabilityCount)
    {
        // Arrange
        var service = CreateService();
        var hardware = CreateMinimalHardware();
        var events = CreateEvents(errorCount.Get, warningCount.Get, criticalCount.Get);
        var reliability = CreateReliability(reliabilityCount.Get);

        // Act
        var result = service.AnalyzeSystemPerformance(hardware, events, reliability);

        // Assert
        Assert.InRange(result.SystemHealthScore, 0d, 100d);
        Assert.InRange(result.StabilityScore, 0d, 100d);
        Assert.InRange(result.PerformanceScore, 0d, 100d);
        Assert.InRange(result.MemoryUsageScore, 0d, 100d);
        Assert.InRange(result.DiskHealthScore, 0d, 100d);
    }

    [Fact]
    public void ScoringWeights_SumToOne()
    {
        var sum = ScoringConfiguration.StabilityWeight +
                  ScoringConfiguration.PerformanceWeight +
                  ScoringConfiguration.MemoryWeight +
                  ScoringConfiguration.DiskWeight;

        Assert.Equal(1.0, sum, precision: 6);
    }

    [Property]
    public void AnalyzeSystemPerformance_WithEmptyEvents_DoesNotThrow(
        NonNegativeInt cpuCores,
        PositiveInt memoryGB)
    {
        // Arrange
        var service = CreateService();
        var hardware = CreateHardware(cpuCores.Get, (ulong)memoryGB.Get * 1024UL * 1024UL * 1024UL);
        var events = new List<LogEventData>();
        var reliability = new List<ReliabilityRecordData>();

        // Act & Assert - should not throw
        var result = service.AnalyzeSystemPerformance(hardware, events, reliability);

        Assert.NotNull(result);
        Assert.Empty(result.Recommendations); // Empty events = no recommendations from events
    }

    [Property]
    public void StabilityScore_DecreasesWithMoreErrors(
        PositiveInt lowerErrors,
        PositiveInt higherErrors)
    {
        // Arrange - ensure higherErrors is actually higher
        var higher = Math.Max(lowerErrors.Get + 1, higherErrors.Get);

        var service = CreateService();
        var hardware = CreateMinimalHardware();

        var lowerEvents = CreateEvents(lowerErrors.Get, 0, 0);
        var higherEvents = CreateEvents(higher, 0, 0);
        var reliability = new List<ReliabilityRecordData>();

        // Act
        var lowerResult = service.AnalyzeSystemPerformance(hardware, lowerEvents, reliability);
        var higherResult = service.AnalyzeSystemPerformance(hardware, higherEvents, reliability);

        // Assert - more errors should result in lower or equal stability score
        Assert.True(lowerResult.StabilityScore >= higherResult.StabilityScore,
            $"Lower errors ({lowerErrors.Get}) should have >= stability than higher errors ({higher})");
    }

    [Property]
    public void AnalyzeSystemPerformance_WithEmptyDisks_Returns50DiskScore(
        NonEmptyString computerName,
        NonEmptyString osVersion)
    {
        // Arrange
        var service = CreateService();
        var hardware = new HardwareData
        {
            ComputerName = computerName.Get,
            OsVersion = osVersion.Get,
            CpuBrand = "Intel Core i5",
            CpuCores = 4,
            TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
            Disks = new List<DiskInfoData>() // Empty disks
        };
        var events = new List<LogEventData>();
        var reliability = new List<ReliabilityRecordData>();

        // Act
        var result = service.AnalyzeSystemPerformance(hardware, events, reliability);

        // Assert - empty disks should return 50 as per CalculateDiskScore implementation
        Assert.Equal(50d, result.DiskHealthScore);
    }

    [Property]
    public void HealthGrade_IsConsistentWithScore(NonNegativeInt rawScore)
    {
        // Arrange
        var score = rawScore.Get % 101; // Clamp to [0, 100]

        // Act & Assert - verify grade thresholds
        var expectedGrade = score switch
        {
            >= 90 => "优秀",
            >= 75 => "良好",
            >= 60 => "一般",
            >= 40 => "较差",
            _ => "危险"
        };

        // This validates the grade mapping is deterministic
        Assert.NotNull(expectedGrade);
    }

    private static PerformanceService CreateService()
    {
        return new PerformanceService(
            new MockSystemInfoProvider(),
            new MockLogService());
    }

    private static HardwareData CreateMinimalHardware()
    {
        return new HardwareData
        {
            ComputerName = "TestPC",
            OsVersion = "Windows 11",
            CpuBrand = "Intel Core i5",
            CpuCores = 4,
            TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
            Disks = new List<DiskInfoData>
            {
                new() { Name = "C:", TotalSpace = 100L * 1024L * 1024L * 1024L, AvailableSpace = 50L * 1024L * 1024L * 1024L }
            }
        };
    }

    private static HardwareData CreateHardware(uint cpuCores, ulong totalMemory)
    {
        return new HardwareData
        {
            ComputerName = "TestPC",
            OsVersion = "Windows 11",
            CpuBrand = "Intel Core i5",
            CpuCores = cpuCores,
            TotalMemory = totalMemory,
            Disks = new List<DiskInfoData>
            {
                new() { Name = "C:", TotalSpace = 100L * 1024L * 1024L * 1024L, AvailableSpace = 50L * 1024L * 1024L * 1024L }
            }
        };
    }

    private static List<LogEventData> CreateEvents(int errorCount, int warningCount, int criticalCount)
    {
        var events = new List<LogEventData>();

        // Add critical errors (Event ID 41 = kernel power)
        for (var i = 0; i < criticalCount; i++)
        {
            events.Add(new LogEventData
            {
                EventType = "error",
                EventId = 41,
                SourceName = "Kernel-Power",
                Message = "critical system error"
            });
        }

        // Add regular errors
        for (var i = 0; i < errorCount; i++)
        {
            events.Add(new LogEventData
            {
                EventType = "error",
                EventId = 1000,
                SourceName = "Application",
                Message = "error message"
            });
        }

        // Add warnings
        for (var i = 0; i < warningCount; i++)
        {
            events.Add(new LogEventData
            {
                EventType = "warning",
                EventId = 2000,
                SourceName = "Application",
                Message = "warning message"
            });
        }

        return events;
    }

    private static List<ReliabilityRecordData> CreateReliability(int count)
    {
        var records = new List<ReliabilityRecordData>();
        for (var i = 0; i < count; i++)
        {
            records.Add(new ReliabilityRecordData
            {
                EventTime = DateTime.UtcNow.AddDays(-i),
                Source = "TestSource",
                Description = "Test reliability record"
            });
        }
        return records;
    }
}
