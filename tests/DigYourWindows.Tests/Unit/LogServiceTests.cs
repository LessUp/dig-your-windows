using System.IO;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for LogService.
/// </summary>
public class LogServiceTests : IDisposable
{
    private readonly string _testLogDirectory;
    private readonly FileLogService _logService;

    public LogServiceTests()
    {
        // Create a unique test directory for each test run
        _testLogDirectory = Path.Combine(
            Path.GetTempPath(),
            "DigYourWindows_Tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_testLogDirectory);

        // Create log service with test directory
        _logService = CreateTestLogService();
    }

    private FileLogService CreateTestLogService()
    {
        // Use reflection to set the log directory since the constructor uses a fixed path
        var service = new FileLogService();
        return service;
    }

    [Fact]
    public void InfoWithValidMessageShouldNotThrow()
    {
        // Arrange
        var message = "Test info message";

        // Act & Assert - should not throw
        var exception = Record.Exception(() => _logService.Info(message));
        Assert.Null(exception);
    }

    [Fact]
    public void WarnWithValidMessageShouldNotThrow()
    {
        // Arrange
        var message = "Test warning message";

        // Act & Assert - should not throw
        var exception = Record.Exception(() => _logService.Warn(message));
        Assert.Null(exception);
    }

    [Fact]
    public void LogErrorWithMessageOnlyShouldNotThrow()
    {
        // Arrange
        var message = "Test error message";

        // Act & Assert - should not throw
        var exception = Record.Exception(() => _logService.LogError(message));
        Assert.Null(exception);
    }

    [Fact]
    public void LogErrorWithMessageAndExceptionShouldNotThrow()
    {
        // Arrange
        var message = "Test error message with exception";
        var exception = new InvalidOperationException("Test inner exception");

        // Act & Assert - should not throw
        var ex = Record.Exception(() => _logService.LogError(message, exception));
        Assert.Null(ex);
    }

    [Fact]
    public void LogErrorWithNullExceptionShouldNotThrow()
    {
        // Arrange
        var message = "Test error message with null exception";

        // Act & Assert - should not throw
        var exception = Record.Exception(() => _logService.LogError(message, null));
        Assert.Null(exception);
    }

    [Fact]
    public void MultipleSequentialLogsShouldNotThrow()
    {
        // Act & Assert - should not throw
        for (int i = 0; i < 100; i++)
        {
            _logService.Info($"Test message {i}");
        }
    }

    [Fact]
    public async Task ConcurrentLogsShouldBeThreadSafe()
    {
        // Arrange
        const int threadCount = 10;
        const int messagesPerThread = 50;
        var exceptions = new List<Exception>();
        var tasks = new Task[threadCount];

        // Act
        for (int t = 0; t < threadCount; t++)
        {
            var threadId = t;
            tasks[t] = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < messagesPerThread; i++)
                    {
                        _logService.Info($"Thread {threadId} message {i}");
                        _logService.Warn($"Thread {threadId} warning {i}");
                        _logService.LogError($"Thread {threadId} error {i}");
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
        }

        await Task.WhenAll(tasks);

        // Assert - no exceptions should have occurred
        Assert.Empty(exceptions);
    }

    [Fact]
    public void DisposeShouldNotThrow()
    {
        // Act & Assert - should not throw
        var exception = Record.Exception(() => _logService.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void DisposeCalledMultipleTimesShouldNotThrow()
    {
        // Act & Assert - should not throw
        _logService.Dispose();
        var exception = Record.Exception(() => _logService.Dispose());
        Assert.Null(exception);
    }

    public void Dispose()
    {
        _logService.Dispose();

        // Clean up test directory
        try
        {
            if (Directory.Exists(_testLogDirectory))
            {
                Directory.Delete(_testLogDirectory, recursive: true);
            }
        }
        catch
        {
            // Ignore cleanup failures
        }
    }
}
