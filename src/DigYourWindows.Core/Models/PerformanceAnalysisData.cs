using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Performance analysis results.
/// </summary>
public record PerformanceAnalysisData
{
    [JsonPropertyName("systemHealthScore")]
    public double SystemHealthScore { get; init; }

    [JsonPropertyName("stabilityScore")]
    public double StabilityScore { get; init; }

    [JsonPropertyName("performanceScore")]
    public double PerformanceScore { get; init; }

    [JsonPropertyName("memoryUsageScore")]
    public double MemoryUsageScore { get; init; }

    [JsonPropertyName("diskHealthScore")]
    public double DiskHealthScore { get; init; }

    [JsonPropertyName("systemUptimeDays")]
    public double? SystemUptimeDays { get; init; }

    [JsonPropertyName("criticalIssuesCount")]
    public uint CriticalIssuesCount { get; init; }

    [JsonPropertyName("warningsCount")]
    public uint WarningsCount { get; init; }

    [JsonPropertyName("recommendations")]
    public List<string> Recommendations { get; init; } = new();

    [JsonPropertyName("healthGrade")]
    public string HealthGrade { get; init; } = string.Empty;

    [JsonPropertyName("healthColor")]
    public string HealthColor { get; init; } = string.Empty;
}
