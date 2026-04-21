using System.Text.Json;

namespace DigYourWindows.Core.Services;

/// <summary>
/// Provides application configuration settings.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Gets the maximum number of log files to retain.
    /// </summary>
    int MaxLogFiles { get; }

    /// <summary>
    /// Gets the maximum log file size in bytes.
    /// </summary>
    long MaxLogFileSizeBytes { get; }

    /// <summary>
    /// Gets the log directory path. If null, uses the default path.
    /// </summary>
    string? LogDirectory { get; }

    /// <summary>
    /// Gets the network history capacity (number of samples to retain).
    /// </summary>
    int NetworkHistoryCapacity { get; }

    /// <summary>
    /// Gets the monitoring timer interval in seconds.
    /// </summary>
    int TimerIntervalSeconds { get; }

    /// <summary>
    /// Gets the maximum length for event messages in reports.
    /// </summary>
    int EventMessageMaxLength { get; }
}

/// <summary>
/// Implementation of configuration service that reads from appsettings.json.
/// </summary>
public sealed class ConfigurationService : IConfigurationService
{
    private readonly ApplicationConfiguration _config;

    public ConfigurationService()
    {
        _config = LoadConfiguration();
    }

    public int MaxLogFiles => _config.Logging.MaxLogFiles;
    public long MaxLogFileSizeBytes => _config.Logging.MaxLogFileSizeBytes;
    public string? LogDirectory => _config.Logging.LogDirectory;
    public int NetworkHistoryCapacity => _config.Monitoring.NetworkHistoryCapacity;
    public int TimerIntervalSeconds => _config.Monitoring.TimerIntervalSeconds;
    public int EventMessageMaxLength => _config.Report.EventMessageMaxLength;

    private static ApplicationConfiguration LoadConfiguration()
    {
        try
        {
            var assemblyPath = typeof(ConfigurationService).Assembly.Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            var configPath = Path.Combine(assemblyDir ?? ".", "appsettings.json");

            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<ApplicationConfiguration>(json) ?? CreateDefault();
            }

            return CreateDefault();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Configuration load failed: {ex.Message}");
            return CreateDefault();
        }
    }

    private static ApplicationConfiguration CreateDefault() => new()
    {
        Logging = new LoggingConfiguration
        {
            MaxLogFiles = 7,
            MaxLogFileSizeBytes = 10 * 1024 * 1024, // 10 MB
            LogDirectory = null
        },
        Monitoring = new MonitoringConfiguration
        {
            NetworkHistoryCapacity = 60,
            TimerIntervalSeconds = 1
        },
        Report = new ReportConfiguration
        {
            EventMessageMaxLength = 100
        }
    };
}

/// <summary>
/// Root configuration class for JSON deserialization.
/// </summary>
internal sealed class ApplicationConfiguration
{
    public LoggingConfiguration Logging { get; set; } = new();
    public MonitoringConfiguration Monitoring { get; set; } = new();
    public ReportConfiguration Report { get; set; } = new();
}

/// <summary>
/// Logging-related configuration.
/// </summary>
internal sealed class LoggingConfiguration
{
    public int MaxLogFiles { get; set; } = 7;
    public long MaxLogFileSizeBytes { get; set; } = 10 * 1024 * 1024;
    public string? LogDirectory { get; set; }
}

/// <summary>
/// Monitoring-related configuration.
/// </summary>
internal sealed class MonitoringConfiguration
{
    public int NetworkHistoryCapacity { get; set; } = 60;
    public int TimerIntervalSeconds { get; set; } = 1;
}

/// <summary>
/// Report-related configuration.
/// </summary>
internal sealed class ReportConfiguration
{
    public int EventMessageMaxLength { get; set; } = 100;
}
