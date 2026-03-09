using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Complete diagnostic data collected from the system.
/// Matches the Rust DiagnosticData structure for cross-version compatibility.
/// </summary>
public record DiagnosticData
{
    [JsonPropertyName("hardware")]
    public HardwareData Hardware { get; init; } = new();

    [JsonPropertyName("reliability")]
    public List<ReliabilityRecordData> Reliability { get; init; } = new();

    [JsonPropertyName("events")]
    public List<LogEventData> Events { get; init; } = new();

    [JsonPropertyName("performance")]
    public PerformanceAnalysisData Performance { get; init; } = new();

    [JsonPropertyName("collectedAt")]
    public DateTime CollectedAt { get; init; } = DateTime.UtcNow;
}
