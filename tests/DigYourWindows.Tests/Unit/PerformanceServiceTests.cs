using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

public class PerformanceServiceTests
{
    private sealed class StubSystemInfoProvider : ISystemInfoProvider
    {
        public double? UptimeDays { get; set; } = 1.0;
        public double? GetSystemUptimeDays() => UptimeDays;
    }

    private sealed class StubLogService : ILogService
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void LogError(string message, Exception? exception = null) { }
    }

    private static PerformanceService CreateService(ISystemInfoProvider? systemInfo = null)
    {
        return new PerformanceService(systemInfo ?? new StubSystemInfoProvider(), new StubLogService());
    }

    [Fact]
    public void AnalyzeSystemPerformance_PoorSystem_ShouldReturnLowGradeAndRecommendations()
    {
        var hardware = new HardwareData
        {
            CpuBrand = "Unknown",
            CpuCores = 1,
            TotalMemory = 2UL * 1024UL * 1024UL * 1024UL,
            Disks =
            [
                new DiskInfoData
                {
                    Name = "C:",
                    FileSystem = "NTFS",
                    TotalSpace = 100UL,
                    AvailableSpace = 5UL
                }
            ]
        };

        var events = new List<LogEventData>
        {
            new()
            {
                TimeGenerated = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LogFile = "System",
                SourceName = "Kernel-Power",
                EventType = "Error",
                EventId = 41,
                Message = "Test critical error"
            }
        };

        var reliability = Enumerable
            .Range(0, 51)
            .Select(i => new ReliabilityRecordData
            {
                Timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMinutes(i),
                SourceName = "TestSource",
                Message = "TestMessage",
                EventType = "Error",
                RecordType = 1
            })
            .ToList();

        var service = CreateService();
        var analysis = service.AnalyzeSystemPerformance(hardware, events, reliability);

        Assert.InRange(analysis.SystemHealthScore, 0d, 100d);
        Assert.True(analysis.SystemHealthScore < 60d);
        Assert.Equal("较差", analysis.HealthGrade);
        Assert.Equal("#fd7e14", analysis.HealthColor);

        Assert.Contains("内存容量严重不足，强烈建议升级到8GB或更多", analysis.Recommendations);
        Assert.Contains("磁盘 C: 剩余空间严重不足 (5%)，请立即清理空间", analysis.Recommendations);
        Assert.Contains("系统可靠性记录较多，建议检查系统稳定性", analysis.Recommendations);
        Assert.Contains("CPU核心数较少，可能会影响多任务处理性能", analysis.Recommendations);
        Assert.Contains("发现 1 个严重系统错误，建议立即检查系统日志", analysis.Recommendations);
        Assert.Contains("系统健康评分较低，建议进行全面系统维护", analysis.Recommendations);

        Assert.Equal((uint)1, analysis.CriticalIssuesCount);
        Assert.Equal((uint)0, analysis.WarningsCount);
    }

    [Fact]
    public void AnalyzeSystemPerformance_HighEndSystem_ShouldReturnExcellentGradeAndNoRecommendations()
    {
        var hardware = new HardwareData
        {
            CpuBrand = "Intel(R) Core(TM) i9-12900K",
            CpuCores = 8,
            TotalMemory = 16UL * 1024UL * 1024UL * 1024UL,
            Disks =
            [
                new DiskInfoData
                {
                    Name = "C:",
                    FileSystem = "NTFS",
                    TotalSpace = 100UL,
                    AvailableSpace = 60UL
                }
            ]
        };

        var service = CreateService();
        var analysis = service.AnalyzeSystemPerformance(hardware, [], []);

        Assert.True(analysis.SystemHealthScore >= 90d);
        Assert.Equal("优秀", analysis.HealthGrade);
        Assert.Equal("#28a745", analysis.HealthColor);
        Assert.Empty(analysis.Recommendations);

        Assert.Equal((uint)0, analysis.CriticalIssuesCount);
        Assert.Equal((uint)0, analysis.WarningsCount);
    }

    [Theory]
    [InlineData(4, 60)]
    [InlineData(8, 75)]
    [InlineData(16, 90)]
    public void AnalyzeSystemPerformance_MemoryThresholds_ShouldReturnExpectedMemoryScores(int memoryGb, double expectedScore)
    {
        var service = CreateService();
        var analysis = service.AnalyzeSystemPerformance(
            CreateHardware(totalMemoryGb: memoryGb),
            [],
            []);

        Assert.Equal(expectedScore, analysis.MemoryUsageScore);
    }

    [Fact]
    public void AnalyzeSystemPerformance_FourGbMemory_ShouldAddUpgradeRecommendation()
    {
        var service = CreateService();
        var analysis = service.AnalyzeSystemPerformance(CreateHardware(totalMemoryGb: 4), [], []);

        Assert.Contains("内存容量较小，建议考虑升级到8GB或更多以提升性能", analysis.Recommendations);
    }

    [Theory]
    [InlineData(10, 30, "严重不足")]
    [InlineData(25, 60, "不足")]
    [InlineData(50, 75, null)]
    public void AnalyzeSystemPerformance_DiskFreeThresholds_ShouldReturnExpectedDiskScores(int freePercent, double expectedScore, string? expectedMessageFragment)
    {
        var service = CreateService();
        var analysis = service.AnalyzeSystemPerformance(
            CreateHardware(diskFreePercent: freePercent),
            [],
            []);

        Assert.Equal(expectedScore, analysis.DiskHealthScore);

        if (expectedMessageFragment is null)
        {
            Assert.DoesNotContain(analysis.Recommendations, recommendation => recommendation.Contains("磁盘 C:"));
        }
        else
        {
            Assert.Contains(analysis.Recommendations, recommendation => recommendation.Contains(expectedMessageFragment, StringComparison.Ordinal));
        }
    }

    [Fact]
    public void AnalyzeSystemPerformance_ReliabilityThresholdBoundary_ShouldOnlyWarnAboveFifty()
    {
        var service = CreateService();
        var fiftyRecords = Enumerable.Range(0, 50).Select(_ => new ReliabilityRecordData()).ToList();
        var fiftyOneRecords = Enumerable.Range(0, 51).Select(_ => new ReliabilityRecordData()).ToList();

        var analysisAtBoundary = service.AnalyzeSystemPerformance(CreateHardware(), [], fiftyRecords);
        var analysisAboveBoundary = service.AnalyzeSystemPerformance(CreateHardware(), [], fiftyOneRecords);

        Assert.DoesNotContain("系统可靠性记录较多，建议检查系统稳定性", analysisAtBoundary.Recommendations);
        Assert.Contains("系统可靠性记录较多，建议检查系统稳定性", analysisAboveBoundary.Recommendations);
        Assert.Equal(100d, analysisAtBoundary.StabilityScore);
        Assert.Equal(90d, analysisAboveBoundary.StabilityScore);
    }

    [Fact]
    public void AnalyzeSystemPerformance_UnknownUptime_ShouldPreserveNullSystemUptime()
    {
        var service = CreateService(new StubSystemInfoProvider { UptimeDays = null });
        var analysis = service.AnalyzeSystemPerformance(CreateHardware(), [], []);

        Assert.Null(analysis.SystemUptimeDays);
    }

    private static HardwareData CreateHardware(int totalMemoryGb = 16, int diskFreePercent = 60)
    {
        return new HardwareData
        {
            CpuBrand = "Intel(R) Core(TM) i5-12400",
            CpuCores = 4,
            TotalMemory = (ulong)totalMemoryGb * 1024UL * 1024UL * 1024UL,
            Disks =
            [
                new DiskInfoData
                {
                    Name = "C:",
                    FileSystem = "NTFS",
                    TotalSpace = 100UL,
                    AvailableSpace = (ulong)diskFreePercent
                }
            ]
        };
    }
}
