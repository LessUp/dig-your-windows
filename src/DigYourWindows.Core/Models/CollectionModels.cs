namespace DigYourWindows.Core.Models;

/// <summary>
/// Represents the progress of a diagnostic collection operation.
/// </summary>
/// <param name="StepIndex">The current step index (0-based).</param>
/// <param name="StepCount">The total number of steps in the collection process.</param>
/// <param name="Message">A human-readable progress message describing the current operation.</param>
public readonly record struct DiagnosticCollectionProgress(int StepIndex, int StepCount, string Message);

/// <summary>
/// Represents the result of a diagnostic collection operation.
/// </summary>
/// <param name="Data">The collected diagnostic data.</param>
/// <param name="Warnings">A list of warnings encountered during collection.</param>
public sealed record DiagnosticCollectionResult(DiagnosticData Data, IReadOnlyList<string> Warnings)
{
    /// <summary>
    /// Gets a value indicating whether any warnings were encountered during collection.
    /// </summary>
    public bool HasWarnings => Warnings.Count > 0;
}
