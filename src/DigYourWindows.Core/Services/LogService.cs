using System.Diagnostics;

namespace DigYourWindows.Core.Services;

public interface ILogService
{
    void Info(string message);
    void Warn(string message);
    void LogError(string message, Exception? exception = null);
}

public sealed class FileLogService : ILogService, IDisposable
{
    private readonly object _lock = new();
    private readonly StreamWriter _writer;
    private bool _disposed;

    public FileLogService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var logDir = Path.Combine(appData, "DigYourWindows", "logs");
        Directory.CreateDirectory(logDir);
        var logFilePath = Path.Combine(logDir, "digyourwindows.log");

        _writer = new StreamWriter(logFilePath, append: true, System.Text.Encoding.UTF8)
        {
            AutoFlush = true
        };
    }

    public void Info(string message) => Write("INFO", message, null);

    public void Warn(string message) => Write("WARN", message, null);

    public void LogError(string message, Exception? exception = null) => Write("ERROR", message, exception);

    private void Write(string level, string message, Exception? exception)
    {
        try
        {
            var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", System.Globalization.CultureInfo.InvariantCulture);
            var line = $"{timestamp} [{level}] {message}";

            if (exception != null)
            {
                line += Environment.NewLine + exception;
            }

            lock (_lock)
            {
                _writer.WriteLine(line);
            }

            Debug.WriteLine(line);
        }
        catch
        {
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            lock (_lock)
            {
                _writer.Dispose();
            }
            _disposed = true;
        }
    }
}
