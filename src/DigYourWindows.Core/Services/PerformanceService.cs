using System.Diagnostics;
using System.Management;
using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface ISystemInfoProvider
{
    double? GetSystemUptimeDays();
}

public class WmiSystemInfoProvider : ISystemInfoProvider
{
    public double? GetSystemUptimeDays()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT LastBootUpTime FROM Win32_OperatingSystem");
            foreach (var obj in searcher.Get())
            {
                using (obj)
                {
                    var lastBoot = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"]?.ToString() ?? string.Empty);
                    var uptime = DateTime.Now - lastBoot;
                    return uptime.TotalDays;
                }
            }
        }
        catch
        {
        }
        return null;
    }
}

public interface IPerformanceService
{
    PerformanceAnalysisData AnalyzeSystemPerformance(
        HardwareData hardware,
        List<LogEventData> events,
        List<ReliabilityRecordData> reliability);
}

public class PerformanceService : IPerformanceService
{
    private static readonly HashSet<uint> CriticalEventIds = new() { 41, 55, 57, 1003, 1073, 6008, 7034, 7036 };

    private const double ExcellentMemoryThresholdGb = 16d;
    private const double GoodMemoryThresholdGb = 8d;
    private const double AcceptableMemoryThresholdGb = 4d;
    private const double ExcellentDiskFreeThresholdPercent = 50d;
    private const double GoodDiskFreeThresholdPercent = 25d;
    private const double AcceptableDiskFreeThresholdPercent = 10d;
    private const int HighReliabilityRecordThreshold = 50;

    private const double StabilityWeight = 0.4d;
    private const double PerformanceWeight = 0.3d;
    private const double MemoryWeight = 0.15d;
    private const double DiskWeight = 0.15d;

    private readonly ISystemInfoProvider _systemInfo;

    public PerformanceService(ISystemInfoProvider systemInfo)
    {
        _systemInfo = systemInfo;
    }

    public PerformanceAnalysisData AnalyzeSystemPerformance(
        HardwareData hardware,
        List<LogEventData> events,
        List<ReliabilityRecordData> reliability)
    {
        var recommendations = new List<string>();
        var eventAnalysis = AnalyzeEvents(events);
        var totalMemoryGB = hardware.TotalMemory / 1024d / 1024d / 1024d;
        var memoryUsageScore = CalculateMemoryScore(totalMemoryGB, recommendations);
        var diskHealthScore = CalculateDiskScore(hardware.Disks, recommendations);
        var systemUptimeDays = _systemInfo.GetSystemUptimeDays();
        var stabilityScore = CalculateStabilityScore(
            eventAnalysis.ErrorCount,
            eventAnalysis.WarningCount,
            eventAnalysis.CriticalEvents.Count,
            reliability.Count,
            recommendations);
        var performanceScore = CalculatePerformanceScore(
            hardware.CpuCores,
            hardware.CpuBrand,
            totalMemoryGB,
            recommendations);
        var systemHealthScore = CalculateSystemHealthScore(
            stabilityScore,
            performanceScore,
            memoryUsageScore,
            diskHealthScore);

        AddSummaryRecommendations(eventAnalysis.CriticalEvents.Count, systemHealthScore, recommendations);

        var (healthGrade, healthColor) = GetHealthGradeAndColor(systemHealthScore);

        return new PerformanceAnalysisData
        {
            SystemHealthScore = systemHealthScore,
            StabilityScore = stabilityScore,
            PerformanceScore = performanceScore,
            MemoryUsageScore = memoryUsageScore,
            DiskHealthScore = diskHealthScore,
            SystemUptimeDays = systemUptimeDays,
            CriticalIssuesCount = (uint)eventAnalysis.CriticalEvents.Count,
            WarningsCount = (uint)eventAnalysis.WarningCount,
            Recommendations = recommendations,
            HealthGrade = healthGrade,
            HealthColor = healthColor
        };
    }

    private static double CalculateMemoryScore(double totalMemoryGB, List<string> recommendations)
    {
        if (totalMemoryGB >= ExcellentMemoryThresholdGb)
        {
            return 90d;
        }

        if (totalMemoryGB >= GoodMemoryThresholdGb)
        {
            return 75d;
        }

        if (totalMemoryGB >= AcceptableMemoryThresholdGb)
        {
            AddRecommendation(recommendations, "内存容量较小，建议考虑升级到8GB或更多以提升性能");
            return 60d;
        }

        AddRecommendation(recommendations, "内存容量严重不足，强烈建议升级到8GB或更多");
        return 40d;
    }

    private static double CalculateDiskScore(List<DiskInfoData> disks, List<string> recommendations)
    {
        if (disks.Count == 0)
        {
            AddRecommendation(recommendations, "未检测到磁盘信息，请检查磁盘连接");
            return 50d;
        }

        var totalScore = 0d;

        foreach (var disk in disks)
        {
            var freePercentage = disk.TotalSpace > 0
                ? (disk.AvailableSpace / (double)disk.TotalSpace) * 100d
                : 0d;

            totalScore += GetDiskScore(disk, freePercentage, recommendations);
        }

        return totalScore / disks.Count;
    }

    private static double GetDiskScore(DiskInfoData disk, double freePercentage, List<string> recommendations)
    {
        if (freePercentage > ExcellentDiskFreeThresholdPercent)
        {
            return 90d;
        }

        if (freePercentage > GoodDiskFreeThresholdPercent)
        {
            return 75d;
        }

        if (freePercentage > AcceptableDiskFreeThresholdPercent)
        {
            AddRecommendation(recommendations, $"磁盘 {disk.Name} 剩余空间不足 ({freePercentage:F0}%)，建议清理空间");
            return 60d;
        }

        AddRecommendation(recommendations, $"磁盘 {disk.Name} 剩余空间严重不足 ({freePercentage:F0}%)，请立即清理空间");
        return 30d;
    }

    private static double CalculateStabilityScore(
        int errorCount,
        int warningCount,
        int criticalEventsCount,
        int reliabilityRecordsCount,
        List<string> recommendations)
    {
        var score = 100d;

        score -= Math.Min(40d, errorCount * 2d);
        score -= Math.Min(20d, warningCount * 0.5d);
        score -= Math.Min(30d, criticalEventsCount * 10d);

        if (reliabilityRecordsCount > HighReliabilityRecordThreshold)
        {
            score -= 10d;
            AddRecommendation(recommendations, "系统可靠性记录较多，建议检查系统稳定性");
        }

        return Math.Max(0d, score);
    }

    private static double CalculatePerformanceScore(
        uint cpuCount,
        string cpuBrand,
        double totalMemoryGB,
        List<string> recommendations)
    {
        var score = 50d;
        score += GetCpuCoreScore(cpuCount, recommendations);
        score += GetCpuBrandScore(cpuBrand);
        score += GetMemoryPerformanceScore(totalMemoryGB);

        return Math.Clamp(score, 0d, 100d);
    }

    private static double GetCpuCoreScore(uint cpuCount, List<string> recommendations)
    {
        if (cpuCount >= 8)
        {
            return 20d;
        }

        if (cpuCount >= 4)
        {
            return 15d;
        }

        if (cpuCount >= 2)
        {
            return 5d;
        }

        AddRecommendation(recommendations, "CPU核心数较少，可能会影响多任务处理性能");
        return -10d;
    }

    private static double GetCpuBrandScore(string cpuBrand)
    {
        var brand = cpuBrand.ToLowerInvariant();

        if (brand.Contains("intel"))
        {
            if (brand.Contains("i9") || brand.Contains("xeon"))
            {
                return 15d;
            }

            if (brand.Contains("i7"))
            {
                return 10d;
            }

            if (brand.Contains("i5"))
            {
                return 5d;
            }
        }
        else if (brand.Contains("amd"))
        {
            if (brand.Contains("ryzen 9") || brand.Contains("threadripper"))
            {
                return 15d;
            }

            if (brand.Contains("ryzen 7"))
            {
                return 10d;
            }

            if (brand.Contains("ryzen 5"))
            {
                return 5d;
            }
        }

        return 0d;
    }

    private static double GetMemoryPerformanceScore(double totalMemoryGB)
    {
        if (totalMemoryGB >= ExcellentMemoryThresholdGb)
        {
            return 15d;
        }

        if (totalMemoryGB >= GoodMemoryThresholdGb)
        {
            return 10d;
        }

        if (totalMemoryGB >= AcceptableMemoryThresholdGb)
        {
            return 5d;
        }

        return -5d;
    }

    private static EventAnalysisResult AnalyzeEvents(List<LogEventData> events)
    {
        var errorCount = 0;
        var warningCount = 0;
        var criticalEvents = new List<LogEventData>();

        foreach (var evt in events)
        {
            switch (evt.EventType.ToLowerInvariant())
            {
                case "error":
                    errorCount++;
                    if (IsCriticalError(evt))
                    {
                        criticalEvents.Add(evt);
                    }
                    break;
                case "warning":
                    warningCount++;
                    break;
            }
        }

        return new EventAnalysisResult
        {
            ErrorCount = errorCount,
            WarningCount = warningCount,
            CriticalEvents = criticalEvents
        };
    }

    private static bool IsCriticalError(LogEventData evt)
    {
        return CriticalEventIds.Contains(evt.EventId) ||
               evt.SourceName.Contains("bugcheck", StringComparison.OrdinalIgnoreCase) ||
               evt.Message.Contains("critical", StringComparison.OrdinalIgnoreCase) ||
               evt.Message.Contains("fatal", StringComparison.OrdinalIgnoreCase);
    }

    private static double CalculateSystemHealthScore(
        double stabilityScore,
        double performanceScore,
        double memoryUsageScore,
        double diskHealthScore)
    {
        var score = stabilityScore * StabilityWeight +
                    performanceScore * PerformanceWeight +
                    memoryUsageScore * MemoryWeight +
                    diskHealthScore * DiskWeight;

        return Math.Clamp(score, 0d, 100d);
    }

    private static void AddSummaryRecommendations(int criticalEventCount, double systemHealthScore, List<string> recommendations)
    {
        if (criticalEventCount > 0)
        {
            AddRecommendation(recommendations, $"发现 {criticalEventCount} 个严重系统错误，建议立即检查系统日志");
        }

        if (systemHealthScore < 60d)
        {
            AddRecommendation(recommendations, "系统健康评分较低，建议进行全面系统维护");
        }
    }

    private static void AddRecommendation(List<string> recommendations, string message)
    {
        if (!recommendations.Contains(message, StringComparer.Ordinal))
        {
            recommendations.Add(message);
        }
    }

    private sealed record EventAnalysisResult
    {
        public int ErrorCount { get; init; }
        public int WarningCount { get; init; }
        public List<LogEventData> CriticalEvents { get; init; } = new();
    }

    private static (string Grade, string Color) GetHealthGradeAndColor(double systemHealthScore)
    {
        return systemHealthScore switch
        {
            >= 90 => ("优秀", "#28a745"),
            >= 75 => ("良好", "#17a2b8"),
            >= 60 => ("一般", "#ffc107"),
            >= 40 => ("较差", "#fd7e14"),
            _ => ("需要优化", "#dc3545")
        };
    }
}
