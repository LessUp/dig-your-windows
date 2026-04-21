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
    private StreamWriter _writer;
    private bool _disposed;
    private DateTime _currentLogFileDate;
    private readonly string _logDirectory;
    private const int MaxLogFiles = 7;
    private const long MaxLogFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    public FileLogService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _logDirectory = Path.Combine(appData, "DigYourWindows", "logs");
        Directory.CreateDirectory(_logDirectory);

        _currentLogFileDate = DateTime.Today;
        var logFilePath = GetLogFilePath(_currentLogFileDate);
        _writer = CreateWriter(logFilePath);

        CleanupOldLogs();
    }

    private static string GetLogFilePath(DateTime date)
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DigYourWindows", "logs",
            $"digyourwindows_{date:yyyyMMdd}.log");
    }

    private static StreamWriter CreateWriter(string path)
    {
        return new StreamWriter(path, append: true, System.Text.Encoding.UTF8)
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
                CheckLogRotation();
                _writer.WriteLine(line);
            }

            Debug.WriteLine(line);
        }
        catch (IOException)
        {
            // Logging failures should never crash the application
        }
        catch (UnauthorizedAccessException)
        {
            // Logging failures should never crash the application
        }
    }

    private void CheckLogRotation()
    {
        var today = DateTime.Today;

        // Check if we need to rotate to a new day's file
        if (today.Date != _currentLogFileDate.Date)
        {
            RotateLogFile(today);
            return;
        }

        // Check if current file exceeds size limit
        try
        {
            var logFilePath = GetLogFilePath(_currentLogFileDate);
            if (File.Exists(logFilePath))
            {
                var fileInfo = new FileInfo(logFilePath);
                if (fileInfo.Length > MaxLogFileSizeBytes)
                {
                    RotateLogFile(today);
                }
            }
        }
        catch (IOException)
        {
            // File size check is non-critical, continue without rotation
        }
        catch (UnauthorizedAccessException)
        {
            // File size check is non-critical, continue without rotation
        }
    }

    private void RotateLogFile(DateTime newDate)
    {
        _writer.Dispose();
        _currentLogFileDate = newDate;
        var newLogPath = GetLogFilePath(newDate);
        _writer = CreateWriter(newLogPath);

        CleanupOldLogs();
    }

    private void CleanupOldLogs()
    {
        try
        {
            var logFiles = Directory.GetFiles(_logDirectory, "digyourwindows_*.log")
                .OrderByDescending(f => f)
                .Skip(MaxLogFiles)
                .ToList();

            foreach (var file in logFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException)
                {
                    // File deletion failure is non-critical
                }
                catch (UnauthorizedAccessException)
                {
                    // File deletion failure is non-critical
                }
            }
        }
        catch (IOException)
        {
            // Directory enumeration failure is non-critical
        }
        catch (UnauthorizedAccessException)
        {
            // Directory enumeration failure is non-critical
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _writer?.Dispose();
        }
    }
}
