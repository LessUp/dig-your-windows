using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for ConfigurationService.
/// </summary>
public class ConfigurationServiceTests
{
    [Fact]
    public void ConfigurationServiceShouldProvideDefaultValues()
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
    public void ConfigurationServiceMaxLogFilesShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.MaxLogFiles > 0);
    }

    [Fact]
    public void ConfigurationServiceMaxLogFileSizeBytesShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.MaxLogFileSizeBytes > 0);
    }

    [Fact]
    public void ConfigurationServiceNetworkHistoryCapacityShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.NetworkHistoryCapacity > 0);
    }

    [Fact]
    public void ConfigurationServiceTimerIntervalSecondsShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.TimerIntervalSeconds > 0);
    }

    [Fact]
    public void ConfigurationServiceEventMessageMaxLengthShouldBePositive()
    {
        // Arrange & Act
        var config = new ConfigurationService();

        // Assert
        Assert.True(config.EventMessageMaxLength > 0);
    }
}
