using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

public class ReportServiceTests
{
    [Fact]
    public void SerializeToJsonThenDeserializeShouldPreserveSelectedFields()
    {
        var service = new ReportService();
        var collectedAt = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = "TEST-PC",
                OsVersion = "Windows 11",
                CpuBrand = "Test CPU",
                CpuCores = 8,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks =
                [
                    new DiskInfoData
                    {
                        Name = "C:",
                        FileSystem = "NTFS",
                        TotalSpace = 100UL,
                        AvailableSpace = 50UL
                    }
                ],
                DiskSmart =
                [
                    new DiskSmartData
                    {
                        DeviceId = "0",
                        FriendlyName = "TestDisk",
                        SerialNumber = "SN123",
                        BusType = 11,
                        MediaType = 4,
                        Size = 512UL * 1024UL * 1024UL * 1024UL,
                        HealthStatus = 1,
                        Temperature = 40,
                        Wear = 5,
                        PowerOnHours = 1234
                    }
                ]
            },
            Reliability =
            [
                new ReliabilityRecordData
                {
                    Timestamp = collectedAt,
                    SourceName = "TestSource",
                    Message = "TestMessage",
                    EventType = "Error",
                    RecordType = 1
                }
            ],
            Events =
            [
                new LogEventData
                {
                    TimeGenerated = collectedAt,
                    LogFile = "System",
                    SourceName = "Kernel-Power",
                    EventType = "Error",
                    EventId = 41,
                    Message = "Test event message"
                }
            ],
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = 80d,
                StabilityScore = 80d,
                PerformanceScore = 80d,
                MemoryUsageScore = 75d,
                DiskHealthScore = 70d,
                SystemUptimeDays = 1d,
                CriticalIssuesCount = 1,
                WarningsCount = 0,
                Recommendations = ["Rec1"],
                HealthGrade = "良好",
                HealthColor = "#17a2b8"
            },
            CollectedAt = collectedAt
        };

        var json = service.SerializeToJson(data, indented: false);
        var deserialized = service.DeserializeFromJson(json);

        Assert.NotNull(deserialized);
        Assert.Equal(data.Hardware.ComputerName, deserialized!.Hardware.ComputerName);
        Assert.Equal(data.Hardware.OsVersion, deserialized.Hardware.OsVersion);
        Assert.Equal(data.Hardware.CpuBrand, deserialized.Hardware.CpuBrand);
        Assert.Equal(data.Hardware.CpuCores, deserialized.Hardware.CpuCores);
        Assert.Equal(data.Hardware.TotalMemory, deserialized.Hardware.TotalMemory);

        Assert.Single(deserialized.Hardware.Disks);
        Assert.Equal("C:", deserialized.Hardware.Disks[0].Name);

        Assert.Single(deserialized.Hardware.DiskSmart);
        Assert.Equal("TestDisk", deserialized.Hardware.DiskSmart[0].FriendlyName);

        Assert.Single(deserialized.Events);
        Assert.Equal((uint)41, deserialized.Events[0].EventId);

        Assert.Single(deserialized.Reliability);
        Assert.Equal((int?)1, deserialized.Reliability[0].RecordType);

        Assert.Equal(80d, deserialized.Performance.SystemHealthScore);
        Assert.Equal(collectedAt, deserialized.CollectedAt);
    }

    [Fact]
    public void SerializeToJsonShouldIncludeDiskSmartProperty()
    {
        var service = new ReportService();

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = "TEST-PC",
                DiskSmart =
                [
                    new DiskSmartData
                    {
                        DeviceId = "0",
                        FriendlyName = "TestDisk"
                    }
                ]
            },
            CollectedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var json = service.SerializeToJson(data, indented: false);

        Assert.Contains("\"diskSmart\"", json);
        Assert.Contains("\"TestDisk\"", json);
    }

    [Fact]
    public void GenerateHtmlReportShouldContainKeySectionsAndTruncateLongEventMessage()
    {
        var service = new ReportService();
        var collectedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var truncatedSuffix = "UNIQUE_SUFFIX_SHOULD_NOT_APPEAR";
        var longMessage = new string('A', 100) + truncatedSuffix;

        var data = CreateDiagnosticData(collectedAt, longMessage: longMessage);

        var html = service.GenerateHtmlReport(data, daysBackForEvents: 3);

        Assert.Contains("Windows 诊断报告", html);
        Assert.Contains("系统概览", html);
        Assert.Contains(data.Hardware.ComputerName, html);
        Assert.Contains(data.Hardware.OsVersion, html);
        Assert.Contains(data.Hardware.CpuName, html);
        Assert.Contains($"{data.Hardware.TotalMemoryMB} MB", html);

        Assert.Contains("系统性能分析", html);
        Assert.Contains(data.Performance.HealthGrade, html);
        Assert.Contains(data.Performance.HealthColor, html);

        Assert.Contains("GPU 信息", html);
        Assert.Contains("GPU1", html);

        Assert.Contains("错误日志 (最近3天)", html);
        Assert.DoesNotContain(truncatedSuffix, html);
    }

    [Fact]
    public void GenerateHtmlReportWithNegativeMaxEventsShouldRenderNoEventRows()
    {
        var service = new ReportService();
        var data = CreateDiagnosticData(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        var html = service.GenerateHtmlReport(data, daysBackForEvents: 3, maxEvents: -1);

        Assert.Contains("错误日志 (最近3天) - 1 条", html);
        Assert.DoesNotContain("Kernel-Power", html);
    }

    [Fact]
    public void GenerateHtmlReportWithoutRecommendationsGpuAndUptimeShouldHideGpuAndShowUnknownUptime()
    {
        var service = new ReportService();
        var data = CreateDiagnosticData(
            new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            includeGpu: false,
            recommendations: [],
            uptimeDays: null);

        var html = service.GenerateHtmlReport(data, daysBackForEvents: 3);

        Assert.DoesNotContain("GPU 信息", html);
        Assert.DoesNotContain("优化建议", html);
        Assert.Contains(">未知<", html);
    }

    [Fact]
    public void GenerateHtmlReportShouldHtmlEncodeSpecialCharacters()
    {
        var service = new ReportService();
        var data = CreateDiagnosticData(
            new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            computerName: "<script>alert('x')</script>",
            eventMessage: "Error <b>bold</b> & details");

        var html = service.GenerateHtmlReport(data, daysBackForEvents: 3);

        Assert.Contains("&lt;script&gt;alert(&#39;x&#39;)&lt;/script&gt;", html);
        Assert.Contains("Error &lt;b&gt;bold&lt;/b&gt; &amp; details", html);
        Assert.DoesNotContain("<script>alert('x')</script>", html);
    }

    private static DiagnosticData CreateDiagnosticData(
        DateTime collectedAt,
        string? longMessage = null,
        string? eventMessage = null,
        string computerName = "TEST-PC",
        bool includeGpu = true,
        List<string>? recommendations = null,
        double? uptimeDays = 1)
    {
        return new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName,
                OsVersion = "Windows 11",
                CpuBrand = "Test CPU",
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Gpus = includeGpu
                    ?
                    [
                        new GpuInfoData
                        {
                            Name = "GPU1",
                            Temperature = 50,
                            Load = 10,
                            MemoryUsed = 100,
                            MemoryTotal = 200,
                            CoreClock = 1000,
                            Power = 50
                        }
                    ]
                    : []
            },
            Events =
            [
                new LogEventData
                {
                    TimeGenerated = collectedAt,
                    LogFile = "System",
                    SourceName = "Kernel-Power",
                    EventType = "Error",
                    EventId = 41,
                    Message = longMessage ?? eventMessage ?? "Test event message"
                }
            ],
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = 88d,
                StabilityScore = 80d,
                PerformanceScore = 80d,
                MemoryUsageScore = 75d,
                DiskHealthScore = 70d,
                CriticalIssuesCount = 1,
                WarningsCount = 0,
                SystemUptimeDays = uptimeDays,
                Recommendations = recommendations ?? ["Rec1"],
                HealthGrade = "良好",
                HealthColor = "#17a2b8"
            },
            CollectedAt = collectedAt
        };
    }
}
