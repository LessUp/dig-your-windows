using System.IO;
using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for SqliteHistoryStoreService.
/// </summary>
public sealed class SqliteHistoryStoreServiceTests : IAsyncLifetime
{
    private string _tempDbPath = string.Empty;
    private MockLogService _logService = null!;
    private SqliteHistoryStoreService _service = null!;

    public async Task InitializeAsync()
    {
        _tempDbPath = Path.Combine(Path.GetTempPath(), $"test-history-{Guid.NewGuid()}.db");
        _logService = new MockLogService();
        _service = new SqliteHistoryStoreService(_tempDbPath, _logService);
        await _service.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        _service?.Dispose();
        if (File.Exists(_tempDbPath))
        {
            File.Delete(_tempDbPath);
        }
    }

    [Fact]
    public async Task InitializeAsync_CreatesDatabase()
    {
        // Assert
        Assert.True(File.Exists(_tempDbPath), "Database file should be created");
    }

    [Fact]
    public async Task SaveAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var diagnosticData = CreateTestDiagnosticData();

        // Act
        var result = await _service.SaveAsync(diagnosticData);

        // Assert
        Assert.True(result, "Save should succeed");
    }

    [Fact]
    public async Task GetMostRecentSummaryAsync_WithNoHistory_ReturnsNull()
    {
        // Act
        var result = await _service.GetMostRecentSummaryAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetMostRecentSummaryAsync_AfterSave_ReturnsSummary()
    {
        // Arrange
        var diagnosticData = CreateTestDiagnosticData();
        await _service.SaveAsync(diagnosticData);

        // Act
        var result = await _service.GetMostRecentSummaryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TEST-PC", result.ComputerName);
        Assert.Equal("A", result.HealthGrade);
    }

    [Fact]
    public async Task ListSummariesAsync_WithMultipleSaves_ReturnsNewestFirst()
    {
        // Arrange
        await _service.SaveAsync(CreateTestDiagnosticData("PC1"));
        await Task.Delay(10); // Ensure different timestamps
        await _service.SaveAsync(CreateTestDiagnosticData("PC2"));

        // Act
        var results = await _service.ListSummariesAsync();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("PC2", results[0].ComputerName); // Newest first
        Assert.Equal("PC1", results[1].ComputerName);
    }

    [Fact]
    public async Task LoadByIdAsync_WithValidId_ReturnsRecord()
    {
        // Arrange
        var diagnosticData = CreateTestDiagnosticData();
        await _service.SaveAsync(diagnosticData);
        var summary = await _service.GetMostRecentSummaryAsync();
        Assert.NotNull(summary);

        // Act
        var result = await _service.LoadByIdAsync(summary.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TEST-PC", result.Summary.ComputerName);
        Assert.NotNull(result.DiagnosticData);
    }

    [Fact]
    public async Task LoadByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _service.LoadByIdAsync("nonexistent-id");

        // Assert
        Assert.Null(result);
    }

    private static DiagnosticData CreateTestDiagnosticData(string computerName = "TEST-PC")
    {
        return new DiagnosticData
        {
            Hardware = new HardwareData
            {
                ComputerName = computerName,
                OsVersion = "Windows 11",
                CpuBrand = "Intel Core i7",
                TotalMemory = 16_000_000_000UL,
                Disks = new List<DiskInfoData> { new() },
            },
            Events = new List<LogEventData> { new() },
            Reliability = new List<ReliabilityRecordData> { new() },
            Performance = new PerformanceAnalysisData { SystemHealthScore = 85.5, HealthGrade = "A" },
            CollectedAt = DateTime.UtcNow
        };
    }
}

/// <summary>
/// Mock implementation of ILogService for tests.
/// </summary>
internal sealed class MockLogService : ILogService
{
    public void Info(string message) { }
    public void Warn(string message) { }
    public void LogError(string message, Exception? exception = null) { }
}
