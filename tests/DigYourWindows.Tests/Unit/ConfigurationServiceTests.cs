using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for ConfigurationService.
/// </summary>
public class ConfigurationServiceTests
{
    [Fact]
    public void ConfigurationService_ShouldProvideDefaultValues()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.Equal(7, config.MaxLogFiles);
        Assert.Equal(10 * 1024 * 1024, config.MaxLogFileSizeBytes);
        Assert.Equal(60, config.NetworkHistoryCapacity);
        Assert.Equal(1, config.TimerIntervalSeconds);
        Assert.Equal(100, config.EventMessageMaxLength);
    }

    [Fact]
    public void ConfigurationService_MaxLogFiles_ShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.MaxLogFiles > 0);
    }

    [Fact]
    public void ConfigurationService_MaxLogFileSizeBytes_ShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.MaxLogFileSizeBytes > 0);
    }

    [Fact]
    public void ConfigurationService_NetworkHistoryCapacity_ShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.NetworkHistoryCapacity > 0);
    }

    [Fact]
    public void ConfigurationService_TimerIntervalSeconds_ShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.TimerIntervalSeconds > 0);
    }

    [Fact]
    public void ConfigurationService_EventMessageMaxLength_ShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.EventMessageMaxLength > 0);
    }
}
