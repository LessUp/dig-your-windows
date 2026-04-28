# Tasks: Add SQLite Diagnostic History

## 1. Core Persistence

- [ ] Add SQLite dependency and configuration for local history storage.
- [ ] Add history models and `IHistoryStoreService`.
- [ ] Implement `SqliteHistoryStoreService` with schema initialization, save, latest, list, and load-by-id APIs.
- [ ] Add tests for persistence behavior and snapshot round-trip.

## 2. Collector Integration

- [ ] Inject `IHistoryStoreService` into `DiagnosticCollectorService`.
- [ ] Persist successful diagnostic results after collection.
- [ ] Surface persistence failures through logging and warnings without failing the current diagnostic result.
- [ ] Add collector integration tests for success and failure paths.

## 3. UI Integration

- [ ] Extend `MainViewModel` to load recent history summary and history list.
- [ ] Add minimal UI entry points for recent diagnostic and history browsing.
- [ ] Reuse existing result application flow to display a selected stored snapshot.
- [ ] Add UI/ViewModel tests for empty, populated, and reload scenarios.

## 4. Spec and Documentation Alignment

- [ ] Update delta specs for features, architecture, and data.
- [ ] Add any directly related user-facing text needed for the new history UI states.
