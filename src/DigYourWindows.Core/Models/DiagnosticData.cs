using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Complete diagnostic data collected from the system.
/// Matches the Rust DiagnosticData structure for cross-version compatibility.
/// </summary>
public record DiagnosticData
{
    /// <summary>
    /// Unique identifier for this diagnostic result (used by history store).
    /// If null, this diagnostic has not yet been persisted to history.
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }

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
