# Proposal: Add SQLite Diagnostic History

## Summary

Introduce a local SQLite-backed diagnostic history store that persists completed `DiagnosticData` snapshots and exposes summary queries for a minimal history list and a "recent diagnostic" entry point in the existing UI.

## Motivation

The application currently supports one-shot collection and export, but it does not keep a built-in local history of completed diagnostics. This prevents users from reopening previous results after restart and blocks future work such as trend analysis and diff views.

## Goals

1. Persist each successful diagnostic collection as a local history record.
2. Store the full `DiagnosticData` payload for later reload.
3. Store summary/index fields for efficient history listing.
4. Surface a minimal history list and a "recent diagnostic" entry point in the UI.
5. Preserve offline-first behavior and avoid external services.

## Non-Goals

1. Trend charts or diff comparison.
2. Automatic retention or cleanup.
3. Cloud sync or multi-device history.
4. Replacing existing HTML/JSON export.
5. Broad schema normalization across all diagnostic sub-objects.

## Scope

This change is intentionally limited to:

- SQLite database initialization and access from the app.
- A history store service in `DigYourWindows.Core`.
- Integration with `DiagnosticCollectorService` to save completed runs.
- Minimal UI affordances for recent diagnostic access and history list browsing.
- Tests covering persistence, reload, and failure handling.

## Impact

- Adds a new post-v1.2 feature area and should be treated as a new OpenSpec change, not a maintenance-only bug fix.
- Introduces a local persistence dependency and a small amount of UI state for browsing history.
- Establishes the storage foundation for future history-based features.
