namespace DigYourWindows.Core.Services;

/// <summary>
/// Configuration constants for performance scoring.
/// Centralizes all thresholds and weights for maintainability.
/// </summary>
public static class ScoringConfiguration
{
    // Memory thresholds (in GB)
    public const double ExcellentMemoryThresholdGb = 16d;
    public const double GoodMemoryThresholdGb = 8d;
    public const double AcceptableMemoryThresholdGb = 4d;

    // Disk free space thresholds (percentage)
    public const double ExcellentDiskFreeThresholdPercent = 50d;
    public const double GoodDiskFreeThresholdPercent = 25d;
    public const double AcceptableDiskFreeThresholdPercent = 10d;

    // Reliability thresholds
    public const int HighReliabilityRecordThreshold = 50;

    // Scoring weights (must sum to 1.0)
    public const double StabilityWeight = 0.4d;
    public const double PerformanceWeight = 0.3d;
    public const double MemoryWeight = 0.15d;
    public const double DiskWeight = 0.15d;

    // Score values
    public const double ExcellentScore = 90d;
    public const double GoodScore = 75d;
    public const double AcceptableScore = 60d;
    public const double PoorScore = 40d;

    // CPU core score contributions
    public const double HighCoreCountScore = 20d;
    public const double MediumCoreCountScore = 15d;
    public const double LowCoreCountScore = 5d;
    public const double SingleCorePenalty = -10d;

    // CPU core count thresholds
    public const int HighCoreCountThreshold = 8;
    public const int MediumCoreCountThreshold = 4;
    public const int LowCoreCountThreshold = 2;

    // CPU brand score contributions
    public const double TopTierCpuScore = 15d;
    public const double HighTierCpuScore = 10d;
    public const double MidTierCpuScore = 5d;

    // Stability scoring penalties
    public const double ErrorPenaltyPerError = 2d;
    public const double MaxErrorPenalty = 40d;
    public const double WarningPenaltyPerWarning = 0.5d;
    public const double MaxWarningPenalty = 20d;
    public const double CriticalEventPenalty = 10d;
    public const double MaxCriticalEventPenalty = 30d;
    public const double HighReliabilityRecordPenalty = 10d;

    // Health grade thresholds
    public const double ExcellentHealthThreshold = 90d;
    public const double GoodHealthThreshold = 75d;
    public const double AcceptableHealthThreshold = 60d;
    public const double PoorHealthThreshold = 40d;

    // Health grade display
    public const string ExcellentHealthGrade = "优秀";
    public const string GoodHealthGrade = "良好";
    public const string AcceptableHealthGrade = "一般";
    public const string PoorHealthGrade = "较差";
    public const string CriticalHealthGrade = "需要优化";

    // Health grade colors
    public const string ExcellentHealthColor = "#28a745";
    public const string GoodHealthColor = "#17a2b8";
    public const string AcceptableHealthColor = "#ffc107";
    public const string PoorHealthColor = "#fd7e14";
    public const string CriticalHealthColor = "#dc3545";
}
