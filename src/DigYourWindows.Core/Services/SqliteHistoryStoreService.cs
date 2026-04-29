using System.Globalization;
using System.Text.Json;
using DigYourWindows.Core.Models;
using Microsoft.Data.Sqlite;

namespace DigYourWindows.Core.Services;

/// <summary>
/// SQLite-backed implementation of IHistoryStoreService.
/// Responsible for schema initialization and CRUD operations.
/// Thread-safe via SqliteConnection's built-in locking.
/// </summary>
public sealed class SqliteHistoryStoreService : IHistoryStoreService
{
    private readonly string _dbPath;
    private readonly ILogService _logService;
    private SqliteConnection? _connection;
    private bool _disposed;

    public SqliteHistoryStoreService(string dbPath, ILogService logService)
    {
        _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = _dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Shared,
            }.ToString();

            _connection = new SqliteConnection(connectionString);
            await _connection.OpenAsync(cancellationToken);

            await CreateSchemaAsync(cancellationToken);
            _logService.Info("History store initialized successfully.");
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to initialize history store: {ex.Message}", ex);
            _connection?.Dispose();
            _connection = null;
            throw;
        }
    }

    public async Task<bool> SaveAsync(DiagnosticData data, CancellationToken cancellationToken = default)
    {
        if (_connection == null)
        {
            _logService.Warn("History store not initialized; cannot save.");
            return false;
        }

        try
        {
            var historyId = Guid.NewGuid().ToString();
            var collectedAtUtc = DateTime.UtcNow;
            var snapshotJson = JsonSerializer.Serialize(data);

            var query = @"
                INSERT INTO diagnostic_history (
                    id, collected_at_utc, computer_name, os_version, cpu_brand,
                    total_memory_bytes, disk_count, event_count, reliability_record_count,
                    system_health_score, health_grade, warning_count, tool_version, snapshot_json
                ) VALUES (
                    @id, @collectedAtUtc, @computerName, @osVersion, @cpuBrand,
                    @totalMemoryBytes, @diskCount, @eventCount, @reliabilityRecordCount,
                    @systemHealthScore, @healthGrade, @warningCount, @toolVersion, @snapshotJson
                )";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", historyId);
                cmd.Parameters.AddWithValue("@collectedAtUtc", collectedAtUtc.ToString("O"));
                cmd.Parameters.AddWithValue("@computerName", data.Hardware.ComputerName ?? "Unknown");
                cmd.Parameters.AddWithValue("@osVersion", data.Hardware.OsVersion ?? "Unknown");
                cmd.Parameters.AddWithValue("@cpuBrand", data.Hardware.CpuBrand ?? "Unknown");
                cmd.Parameters.AddWithValue("@totalMemoryBytes", (long)data.Hardware.TotalMemory);
                cmd.Parameters.AddWithValue("@diskCount", data.Hardware.Disks.Count);
                cmd.Parameters.AddWithValue("@eventCount", data.Events.Count);
                cmd.Parameters.AddWithValue("@reliabilityRecordCount", data.Reliability.Count);
                cmd.Parameters.AddWithValue("@systemHealthScore", data.Performance.SystemHealthScore);
                cmd.Parameters.AddWithValue("@healthGrade", data.Performance.HealthGrade ?? "Unknown");
                cmd.Parameters.AddWithValue("@warningCount", (int)data.Performance.WarningsCount);
                cmd.Parameters.AddWithValue("@toolVersion", "1.2.0");
                cmd.Parameters.AddWithValue("@snapshotJson", snapshotJson);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            _logService.Info($"Diagnostic saved to history with id: {historyId}");
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to save diagnostic to history: {ex.Message}", ex);
            return false;
        }
    }

    public async Task<DiagnosticHistorySummary?> GetMostRecentSummaryAsync(CancellationToken cancellationToken = default)
    {
        if (_connection == null) return null;

        try
        {
            var query = @"
                SELECT id, collected_at_utc, computer_name, os_version, cpu_brand,
                       total_memory_bytes, disk_count, event_count, reliability_record_count,
                       system_health_score, health_grade, warning_count, tool_version
                FROM diagnostic_history
                ORDER BY collected_at_utc DESC
                LIMIT 1";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return ReadSummaryFromReader(reader);
                    }
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to read most recent summary: {ex.Message}", ex);
            return null;
        }
    }

    public async Task<IReadOnlyList<DiagnosticHistorySummary>> ListSummariesAsync(CancellationToken cancellationToken = default)
    {
        if (_connection == null) return Array.Empty<DiagnosticHistorySummary>();

        try
        {
            var summaries = new List<DiagnosticHistorySummary>();
            var query = @"
                SELECT id, collected_at_utc, computer_name, os_version, cpu_brand,
                       total_memory_bytes, disk_count, event_count, reliability_record_count,
                       system_health_score, health_grade, warning_count, tool_version
                FROM diagnostic_history
                ORDER BY collected_at_utc DESC";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        summaries.Add(ReadSummaryFromReader(reader));
                    }
                }
            }

            return summaries;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to list summaries: {ex.Message}", ex);
            return Array.Empty<DiagnosticHistorySummary>();
        }
    }

    public async Task<DiagnosticHistoryRecord?> LoadByIdAsync(string historyId, CancellationToken cancellationToken = default)
    {
        if (_connection == null) return null;

        try
        {
            var query = @"
                SELECT id, collected_at_utc, computer_name, os_version, cpu_brand,
                       total_memory_bytes, disk_count, event_count, reliability_record_count,
                       system_health_score, health_grade, warning_count, tool_version, snapshot_json
                FROM diagnostic_history
                WHERE id = @id";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", historyId);

                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        var summary = ReadSummaryFromReader(reader);
                        var snapshotJson = reader.GetString(12);
                        var diagnosticData = JsonSerializer.Deserialize<DiagnosticData>(snapshotJson);

                        if (diagnosticData != null)
                        {
                            diagnosticData.Id = historyId;
                            return new DiagnosticHistoryRecord
                            {
                                Summary = summary,
                                DiagnosticData = diagnosticData,
                            };
                        }
                    }
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to load record {historyId}: {ex.Message}", ex);
            return null;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _connection?.Dispose();
        _disposed = true;
    }

    private async Task CreateSchemaAsync(CancellationToken cancellationToken = default)
    {
        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS diagnostic_history (
                id TEXT PRIMARY KEY,
                collected_at_utc TEXT NOT NULL,
                computer_name TEXT NOT NULL,
                os_version TEXT NOT NULL,
                cpu_brand TEXT NOT NULL,
                total_memory_bytes INTEGER NOT NULL,
                disk_count INTEGER NOT NULL,
                event_count INTEGER NOT NULL,
                reliability_record_count INTEGER NOT NULL,
                system_health_score REAL NOT NULL,
                health_grade TEXT NOT NULL,
                warning_count INTEGER NOT NULL,
                tool_version TEXT NOT NULL,
                snapshot_json TEXT NOT NULL
            );
            CREATE INDEX IF NOT EXISTS idx_diagnostic_history_collected_at 
                ON diagnostic_history(collected_at_utc DESC);";

        using (var cmd = _connection!.CreateCommand())
        {
            cmd.CommandText = createTableSql;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static DiagnosticHistorySummary ReadSummaryFromReader(SqliteDataReader reader)
    {
        return new DiagnosticHistorySummary
        {
            Id = reader.GetString(0),
            CollectedAtUtc = DateTime.Parse(reader.GetString(1), CultureInfo.InvariantCulture, styles: DateTimeStyles.AdjustToUniversal),
            ComputerName = reader.GetString(2),
            OsVersion = reader.GetString(3),
            CpuBrand = reader.GetString(4),
            TotalMemoryBytes = reader.GetInt64(5),
            DiskCount = reader.GetInt32(6),
            EventCount = reader.GetInt32(7),
            ReliabilityRecordCount = reader.GetInt32(8),
            SystemHealthScore = reader.GetDouble(9),
            HealthGrade = reader.GetString(10),
            WarningCount = reader.GetInt32(11),
            ToolVersion = reader.GetString(12),
        };
    }
}
