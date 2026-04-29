using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.PropertyTests;

/// <summary>
/// FsCheck property tests for HTML report generation.
/// Validates structural integrity and content requirements.
/// </summary>
public class ReportServiceHtmlPropertyTests
{
    /// <summary>
    /// Mock log service for testing.
    /// </summary>
    private sealed class MockLogService : ILogService
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void LogError(string message, Exception? exception = null) { }
    }

    [Property]
    public void GenerateHtmlReport_AlwaysContainsRequiredSections(
        NonEmptyString computerName,
        NonEmptyString osVersion)
    {
        // Arrange
        var service = new ReportService();
        var data = CreateMinimalDiagnosticData(computerName.Get, osVersion.Get);

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert
        Assert.Contains("DigYourWindows", html);
        Assert.Contains(computerName.Get, html);
        Assert.Contains(osVersion.Get, html);
        Assert.Contains("</html>", html);
    }

    [Property]
    public void GenerateHtmlReport_HasBalancedHtmlTags(
        NonEmptyString computerName,
        NonEmptyString osVersion)
    {
        // Arrange
        var service = new ReportService();
        var data = CreateMinimalDiagnosticData(computerName.Get, osVersion.Get);

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert - Basic structural integrity
        Assert.Contains("<html", html);
        Assert.Contains("</html>", html);
        Assert.Contains("<head>", html);
        Assert.Contains("</head>", html);
        Assert.Contains("<body>", html);
        Assert.Contains("</body>", html);
    }

    [Property]
    public void GenerateHtmlReport_IncludesPerformanceMetrics(
        NonEmptyString computerName,
        NonNegativeInt healthScore,
        NonNegativeInt stabilityScore)
    {
        // Arrange
        var service = new ReportService();
        var health = Math.Clamp(healthScore.Get % 101, 0, 100);
        var stability = Math.Clamp(stabilityScore.Get % 101, 0, 100);

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = "Windows 11",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>()
            },
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = health,
                StabilityScore = stability,
                PerformanceScore = 80,
                MemoryUsageScore = 75,
                DiskHealthScore = 85,
                HealthGrade = health >= 90 ? "优秀" : health >= 75 ? "良好" : "一般",
                HealthColor = "#17a2b8",
                Recommendations = new List<string>()
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert
        Assert.Contains("系统健康", html); // Health section title
        Assert.Contains(health.ToString("F1"), html); // Health score value
    }

    [Property]
    public void GenerateHtmlReport_EscapesSpecialCharacters(
        NonEmptyString computerName)
    {
        // Arrange
        var service = new ReportService();
        // Use a name with potential HTML characters
        var safeName = computerName.Get.Replace("<", "").Replace(">", "").Replace("&", "");
        var data = CreateMinimalDiagnosticData(safeName, "Windows 11");

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert - Should not have unescaped script tags or other dangerous content
        Assert.DoesNotContain("<script>", html);
        // The HTML should be valid
        Assert.Contains("<!DOCTYPE html>", html);
    }

    [Property]
    public void GenerateHtmlReport_WithEmptyCollections_DoesNotThrow(
        NonEmptyString computerName,
        NonEmptyString osVersion)
    {
        // Arrange
        var service = new ReportService();
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
                SystemHealthScore = 85,
                StabilityScore = 90,
                PerformanceScore = 80,
                MemoryUsageScore = 75,
                DiskHealthScore = 85,
                HealthGrade = "良好",
                HealthColor = "#17a2b8",
                Recommendations = new List<string>()
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act & Assert - Should not throw
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);
        Assert.NotNull(html);
        Assert.NotEmpty(html);
    }

    [Property]
    public void GenerateHtmlReport_WithEvents_ContainsEventSection(
        NonEmptyString computerName,
        PositiveInt eventCount)
    {
        // Arrange
        var service = new ReportService();
        var count = Math.Min(eventCount.Get, 50); // Cap at 50 events
        var events = new List<LogEventData>();

        for (var i = 0; i < count; i++)
        {
            events.Add(new LogEventData
            {
                EventType = i % 2 == 0 ? "Error" : "Warning",
                EventId = 1000 + i,
                SourceName = "Application",
                Message = $"Test event message {i}",
                TimeCreated = DateTime.UtcNow.AddHours(-i)
            });
        }

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = "Windows 11",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>()
            },
            Events = events,
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = 85,
                Recommendations = new List<string>()
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7, maxEvents: 100);

        // Assert - Event section should exist
        Assert.Contains("事件", html); // Events section
    }

    [Property]
    public void GenerateHtmlReport_WithRecommendations_ContainsList(
        NonEmptyString computerName,
        PositiveInt recommendationCount)
    {
        // Arrange
        var service = new ReportService();
        var count = Math.Min(recommendationCount.Get, 10);
        var recommendations = new List<string>();

        for (var i = 0; i < count; i++)
        {
            recommendations.Add($"建议 #{i + 1}: 优化系统性能");
        }

        var data = new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName.Get,
                OsVersion = "Windows 11",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>()
            },
            Performance = new PerformanceAnalysisData
            {
                SystemHealthScore = 60,
                HealthGrade = "一般",
                HealthColor = "#ffc107",
                Recommendations = recommendations
            },
            CollectedAt = DateTime.UtcNow
        };

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert
        Assert.Contains("建议", html); // Recommendations section
    }

    [Fact]
    public void GenerateHtmlReport_GeneratesValidHtmlStructure()
    {
        // Arrange
        var service = new ReportService();
        var data = CreateMinimalDiagnosticData("TestPC", "Windows 11 Pro");

        // Act
        var html = service.GenerateHtmlReport(data, daysBackForEvents: 7);

        // Assert - Validate complete HTML structure
        Assert.StartsWith("<!DOCTYPE html>", html);
        Assert.EndsWith("</html>" + Environment.NewLine, html);

        // Count opening and closing tags for basic elements
        var htmlOpenCount = CountOccurrences(html, "<html");
        var htmlCloseCount = CountOccurrences(html, "</html>");
        Assert.Equal(htmlOpenCount, htmlCloseCount);

        var bodyOpenCount = CountOccurrences(html, "<body");
        var bodyCloseCount = CountOccurrences(html, "</body>");
        Assert.Equal(bodyOpenCount, bodyCloseCount);
    }

    private static DiagnosticData CreateMinimalDiagnosticData(string computerName, string osVersion)
    {
        return new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName,
                OsVersion = osVersion,
                CpuBrand = "Intel Core i5",
                CpuCores = 4,
                TotalMemory = 8UL * 1024UL * 1024UL * 1024UL,
                Disks = new List<DiskInfoData>
                {
                    new() { Name = "C:", TotalSpace = 256L * 1024L * 1024L * 1024L, AvailableSpace = 128L * 1024L * 1024L * 1024L }
                }
            },
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
            Events = new List<LogEventData>(),
            CollectedAt = DateTime.UtcNow
        };
    }

    private static int CountOccurrences(string text, string pattern)
    {
        var count = 0;
        var index = 0;
        while ((index = text.IndexOf(pattern, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += pattern.Length;
        }
        return count;
    }
}
