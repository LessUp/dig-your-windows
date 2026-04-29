# Design: SQLite Diagnostic History

## Overview

The design adds a dedicated history subsystem that is responsible for local persistence and retrieval of completed diagnostics. The subsystem is isolated from collection and export logic so that future history-based features can evolve without overloading existing services.

## Architecture

### New Core Abstractions

- `IHistoryStoreService`
  - Initialize storage.
  - Save a completed `DiagnosticData` snapshot.
  - Read history summaries ordered by newest first.
  - Read the most recent history summary.
  - Load a persisted snapshot by history id.

- `SqliteHistoryStoreService`
  - SQLite-backed implementation of `IHistoryStoreService`.
  - Responsible for schema initialization and CRUD operations.

- `DiagnosticHistorySummary`
  - Lightweight model for the history list and recent diagnostic entry.

- `DiagnosticHistoryRecord`
  - Full persisted record containing summary fields and the reloaded `DiagnosticData`.

### Integration Points

- `App.xaml.cs`
  - Register the history store service in DI.

- `DiagnosticCollectorService`
  - After a successful collection result is assembled, attempt to persist it.
  - Persistence failures do not invalidate the just-collected diagnostic result.
  - Persistence failures are logged and added to warnings.

- `MainViewModel`
  - Load most recent history summary on startup.
  - Load a history summary list for browsing.
  - Allow reloading a selected stored snapshot through existing result application flow.

## Storage Design

### Database Strategy

Use a single SQLite database file with a single primary table in the first version. This keeps the implementation small and optimizes for product value over relational completeness.

### Table: `diagnostic_history`

- `id` TEXT PRIMARY KEY
- `collected_at_utc` TEXT NOT NULL
- `computer_name` TEXT NOT NULL
- `os_version` TEXT NOT NULL
- `cpu_brand` TEXT NOT NULL
- `total_memory_bytes` INTEGER NOT NULL
- `disk_count` INTEGER NOT NULL
- `event_count` INTEGER NOT NULL
- `reliability_record_count` INTEGER NOT NULL
- `system_health_score` REAL NOT NULL
- `health_grade` TEXT NOT NULL
- `warning_count` INTEGER NOT NULL
- `tool_version` TEXT NOT NULL
- `snapshot_json` TEXT NOT NULL

### Rationale

- Summary fields enable efficient list queries without deserializing the full snapshot.
- `snapshot_json` preserves the complete diagnostic payload and allows reusing existing rendering logic.
- A single-table design is sufficient for the first version and leaves room for future derived tables or indexes.

## UI Flow

### Save Flow

1. User runs diagnostics.
2. `DiagnosticCollectorService` collects data as it does today.
3. The completed `DiagnosticData` is saved to the history store.
4. The current result remains available even if persistence fails.

### Startup Flow

1. App starts.
2. `MainViewModel` requests the most recent summary and history list.
3. If present, the UI shows a "recent diagnostic" entry point and a history list.
4. If absent, the UI shows an empty history state without failing startup.

### Reload Flow

1. User selects a history entry.
2. The history store loads and deserializes `snapshot_json`.
3. The existing result application flow displays the stored result.

## Error Handling

- Database initialization failure:
  - Log explicitly.
  - Disable history features for the session.
  - Do not prevent main UI startup.

- Save failure:
  - Log explicitly.
  - Add a warning to the current collection result.
  - Do not discard the in-memory diagnostic result.

- Read/deserialization failure:
  - Log explicitly.
  - Keep history list stable where possible.
  - Surface empty or failed state in UI instead of crashing.

## Testing Strategy

1. `SqliteHistoryStoreService` tests
   - Initialize database.
   - Save snapshot.
   - Read summaries newest first.
   - Read most recent summary.
   - Load snapshot by id.

2. Serialization round-trip tests
   - Persisted `snapshot_json` rehydrates into equivalent core fields.

3. Collector integration tests
   - Successful collection persists history.
   - Persistence failure adds warning and preserves result.

4. ViewModel tests
   - Startup with empty history.
   - Startup with existing recent record.
   - Reload selected history item.

## Future Extension Points

- Retention policy.
- Trend and diff views.
- Secondary indexes for analytics.
- Derived tables for historical aggregations.
