namespace DigYourWindows.Core.Services;

/// <summary>
/// Abstraction for diagnostic history storage and retrieval.
/// Implementations must be thread-safe and support cancellation.
/// </summary>
public interface IHistoryStoreService : IDisposable
{
    /// <summary>
    /// Initialize or verify storage backend.
    /// Called once during app startup.
    /// Failures should not prevent app startup but should disable history features.
    /// </summary>
    Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persist a completed diagnostic snapshot.
    /// Non-blocking: failures are logged but do not invalidate the in-memory result.
    /// </summary>
    Task<bool> SaveAsync(DiagnosticData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Load the most recent diagnostic summary without deserializing the full snapshot.
    /// Returns null if history is empty.
    /// </summary>
    Task<DiagnosticHistorySummary?> GetMostRecentSummaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Load all diagnostic summaries ordered by newest first.
    /// Returns empty list if history is empty.
    /// </summary>
    Task<IReadOnlyList<DiagnosticHistorySummary>> ListSummariesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Load a complete diagnostic record (summary + snapshot) by history id.
    /// Returns null if not found.
    /// </summary>
    Task<DiagnosticHistoryRecord?> LoadByIdAsync(string historyId, CancellationToken cancellationToken = default);
}
