using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for NetworkMonitorService.
/// Note: These tests verify the service works correctly on the current machine.
/// </summary>
public class NetworkMonitorServiceTests
{
    private sealed class StubLogService : ILogService
    {
        public List<string> WarnMessages { get; } = [];
        public void Info(string message) { }
        public void Warn(string message) => WarnMessages.Add(message);
        public void LogError(string message, Exception? exception = null) { }
    }

    private readonly StubLogService _log = new();

    [Fact]
    public void GetTotalBytesShouldReturnNonNegativeValues()
    {
        // Arrange
        var service = new NetworkMonitorService(_log);

        // Act
        var (bytesReceived, bytesSent) = service.GetTotalBytes();

        // Assert
        Assert.True(bytesReceived >= 0);
        Assert.True(bytesSent >= 0);
    }

    [Fact]
    public void GetTotalBytesCalledMultipleTimesShouldReturnIncreasingOrSameValues()
    {
        // Arrange
        var service = new NetworkMonitorService(_log);

        // Act
        var (received1, sent1) = service.GetTotalBytes();
        var (received2, sent2) = service.GetTotalBytes();

        // Assert - values should be same or increase (network activity)
        Assert.True(received2 >= received1);
        Assert.True(sent2 >= sent1);
    }

    [Fact]
    public void GetTotalBytesShouldNotLogWarningsUnderNormalConditions()
    {
        // Arrange
        var service = new NetworkMonitorService(_log);

        // Act
        service.GetTotalBytes();

        // Assert - under normal conditions, no warnings should be logged
        // Note: This may fail if there are problematic network interfaces
        // but that's a valid test scenario
        Assert.Empty(_log.WarnMessages);
    }

    [Fact]
    public void GetTotalBytesReturnedTupleShouldHaveCorrectStructure()
    {
        // Arrange
        var service = new NetworkMonitorService(_log);

        // Act
        var result = service.GetTotalBytes();

        // Assert - verify the tuple structure
        Assert.IsType<(long, long)>(result);
    }
}
