namespace DigYourWindows.Core.Models;

/// <summary>
/// Complete persisted record: summary fields + deserialized snapshot.
/// Used when reloading a stored diagnostic for display.
/// </summary>
public sealed record DiagnosticHistoryRecord
{
    /// <summary>
    /// Summary fields for list/recent display.
    /// </summary>
    public required DiagnosticHistorySummary Summary { get; init; }

    /// <summary>
    /// Full diagnostic snapshot ready for display.
    /// </summary>
    public required DiagnosticData DiagnosticData { get; init; }
}
