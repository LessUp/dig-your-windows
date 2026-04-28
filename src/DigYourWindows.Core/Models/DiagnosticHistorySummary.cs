namespace DigYourWindows.Core.Models;

/// <summary>
/// Lightweight summary of a stored diagnostic for list display and recent entry.
/// Avoids deserializing the full snapshot JSON for UI binding.
/// </summary>
public sealed record DiagnosticHistorySummary
{
    /// <summary>
    /// Unique identifier for this history record.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// UTC timestamp when diagnostic was collected.
    /// </summary>
    public required DateTime CollectedAtUtc { get; init; }

    /// <summary>
    /// Computer name from diagnostic.
    /// </summary>
    public required string ComputerName { get; init; }

    /// <summary>
    /// OS version from diagnostic.
    /// </summary>
    public required string OsVersion { get; init; }

    /// <summary>
    /// CPU brand/model from diagnostic.
    /// </summary>
    public required string CpuBrand { get; init; }

    /// <summary>
    /// Total system memory in bytes.
    /// </summary>
    public required long TotalMemoryBytes { get; init; }

    /// <summary>
    /// Number of disk devices.
    /// </summary>
    public required int DiskCount { get; init; }

    /// <summary>
    /// Number of event log entries collected.
    /// </summary>
    public required int EventCount { get; init; }

    /// <summary>
    /// Number of reliability records.
    /// </summary>
    public required int ReliabilityRecordCount { get; init; }

    /// <summary>
    /// System health score (0.0 - 100.0).
    /// </summary>
    public required double SystemHealthScore { get; init; }

    /// <summary>
    /// Health grade (e.g., "A", "B", "C", "D", "F").
    /// </summary>
    public required string HealthGrade { get; init; }

    /// <summary>
    /// Number of warnings in diagnostic.
    /// </summary>
    public required int WarningCount { get; init; }

    /// <summary>
    /// Tool version at time of collection.
    /// </summary>
    public required string ToolVersion { get; init; }
}
