using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Integration;

/// <summary>
/// Integration tests that verify multiple services work together correctly.
/// These tests focus on the interaction between components rather than individual units.
/// </summary>
public class ServiceIntegrationTests
{
    private sealed class StubLogService : ILogService
    {
        public List<string> InfoMessages { get; } = [];
        public List<string> WarnMessages { get; } = [];
        public List<(string Message, Exception? Exception)> ErrorMessages { get; } = [];

        public void Info(string message) => InfoMessages.Add(message);
        public void Warn(string message) => WarnMessages.Add(message);
        public void LogError(string message, Exception? exception = null) => ErrorMessages.Add((message, exception));
    }

    private readonly StubLogService _log = new();

    [Fact]
    public void ReportServiceSerializePerformanceAnalysisResultShouldRoundtripCorrectly()
    {
        // Arrange
        var reportService = new ReportService();
        var performanceService = new PerformanceService(
            new StubSystemInfoProvider { UptimeDays = 5.5 },
            _log);

        var hardware = new HardwareData
        {
            ComputerName = "INTEGRATION-TEST",
            OsVersion = "Windows 11 Pro",
            CpuBrand = "Intel Core i7-12700K",
            CpuCores = 12,
            TotalMemory = 32UL * 1024UL * 1024UL * 1024UL,
            Disks =
            [
                new DiskInfoData
                {
                    Name = "C:",
                    FileSystem = "NTFS",
                    TotalSpace = 1000UL * 1024UL * 1024UL * 1024UL,
                    AvailableSpace = 600UL * 1024UL * 1024UL * 1024UL
                }
            ]
        };

        var events = new List<LogEventData>
        {
            new()
            {
                TimeGenerated = DateTime.UtcNow.AddDays(-1),
                LogFile = "System",
                SourceName = "EventLog",
                EventType = "Error",
                EventId = 1000,
                Message = "Integration test event"
            }
        };

        var reliability = new List<ReliabilityRecordData>
        {
            new()
            {
                Timestamp = DateTime.UtcNow.AddDays(-2),
                SourceName = "Windows Error Reporting",
                Message = "Application crash",
                EventType = "Error",
                RecordType = 1
            }
        };

        // Act
        var analysis = performanceService.AnalyzeSystemPerformance(hardware, events, reliability);

        var diagnosticData = new DiagnosticData
        {
            Hardware = hardware,
            Events = events,
            Reliability = reliability,
            Performance = analysis,
            CollectedAt = DateTime.UtcNow
        };

        var json = reportService.SerializeToJson(diagnosticData);
        var deserialized = reportService.DeserializeFromJson(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(hardware.ComputerName, deserialized!.Hardware.ComputerName);
        Assert.Equal(analysis.SystemHealthScore, deserialized.Performance.SystemHealthScore);
        Assert.Equal(analysis.HealthGrade, deserialized.Performance.HealthGrade);
        Assert.Single(deserialized.Events);
        Assert.Single(deserialized.Reliability);
    }

    [Fact]
    public void ReportService_GenerateHtmlReport_WithPerformanceAnalysis_ShouldContainAllSections()
    {
        // Arrange
        var reportService = new ReportService();
        var performanceService = new PerformanceService(
            new StubSystemInfoProvider { UptimeDays = 10.0 },
            _log);

        var hardware = new HardwareData
        {
            ComputerName = "HTML-TEST-PC",
            OsVersion = "Windows 11",
            CpuBrand = "AMD Ryzen 9 5950X",
            TotalMemory = 64UL * 1024UL * 1024UL * 1024UL,
            Gpus =
            [
                new GpuInfoData
                {
                    Name = "NVIDIA GeForce RTX 4090",
                    Temperature = 65.5f,
                    Load = 45.0f,
                    MemoryUsed = 8192,
                    MemoryTotal = 24576,
                    CoreClock = 2500,
                    Power = 350
                }
            ]
        };

        var analysis = performanceService.AnalyzeSystemPerformance(hardware, [], []);

        var data = new DiagnosticData
        {
            Hardware = hardware,
            Performance = analysis,
            CollectedAt = new DateTime(2025, 6, 15, 12, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var html = reportService.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert
        Assert.Contains("HTML-TEST-PC", html);
        Assert.Contains("AMD Ryzen 9 5950X", html);
        Assert.Contains("系统性能分析", html);
        Assert.Contains(analysis.HealthGrade, html);
        Assert.Contains("GPU 信息", html);
        Assert.Contains("RTX 4090", html);
        Assert.Contains("系统运行时间", html);
        Assert.Contains("10 天", html);
    }

    [Fact]
    public void PerformanceService_WithHighEndSystem_ShouldProduceExcellentScore()
    {
        // Arrange
        var performanceService = new PerformanceService(
            new StubSystemInfoProvider { UptimeDays = 30.0 },
            _log);

        var hardware = new HardwareData
        {
            CpuBrand = "Intel Core i9-14900K",
            CpuCores = 24,
            TotalMemory = 128UL * 1024UL * 1024UL * 1024UL, // 128 GB
            Disks =
            [
                new DiskInfoData
                {
                    Name = "C:",
                    FileSystem = "NTFS",
                    TotalSpace = 2000UL * 1024UL * 1024UL * 1024UL,
                    AvailableSpace = 1500UL * 1024UL * 1024UL * 1024UL // 75% free
                }
            ]
        };

        // Act
        var analysis = performanceService.AnalyzeSystemPerformance(hardware, [], []);

        // Assert
        Assert.True(analysis.SystemHealthScore >= 90);
        Assert.Equal("优秀", analysis.HealthGrade);
        Assert.Empty(analysis.Recommendations);
        Assert.Equal(30.0, analysis.SystemUptimeDays);
    }

    private sealed class StubSystemInfoProvider : ISystemInfoProvider
    {
        public double? UptimeDays { get; set; } = 1.0;
        public double? GetSystemUptimeDays() => UptimeDays;
    }
}
