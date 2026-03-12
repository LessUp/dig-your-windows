using System.Diagnostics.Eventing.Reader;
using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface IEventLogService
{
    List<LogEventData> GetErrorEvents(int daysBack = 3, CancellationToken cancellationToken = default);
}

public class EventLogService : IEventLogService
{
    private readonly ILogService _log;

    public EventLogService(ILogService log)
    {
        _log = log;
    }

    public List<LogEventData> GetErrorEvents(int daysBack = 3, CancellationToken cancellationToken = default)
    {
        var events = new List<LogEventData>();
        var cutoffDate = DateTime.UtcNow.AddDays(-daysBack);

        try
        {
            // System Log
            events.AddRange(ReadEventLog("System", cutoffDate, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();

            // Application Log
            events.AddRange(ReadEventLog("Application", cutoffDate, cancellationToken));
        }
        catch (Exception ex)
        {
            _log.LogError("读取事件日志失败", ex);
        }

        return events.OrderByDescending(e => e.TimeGenerated).ToList();
    }

    private List<LogEventData> ReadEventLog(string logName, DateTime cutoffDateUtc, CancellationToken cancellationToken)
    {
        var entries = new List<LogEventData>();

        try
        {
            // Use structured XML query for efficient server-side filtering:
            //   Level 2 = Error, Level 3 = Warning
            var cutoffStr = cutoffDateUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ", System.Globalization.CultureInfo.InvariantCulture);
            var queryXml = $"<QueryList><Query Id='0' Path='{logName}'>" +
                           $"<Select Path='{logName}'>*[System[(Level=2 or Level=3) and " +
                           $"TimeCreated[@SystemTime&gt;='{cutoffStr}']]]</Select>" +
                           "</Query></QueryList>";

            using var reader = new EventLogReader(new EventLogQuery(logName, PathType.LogName, queryXml));

            EventRecord? record;
            while ((record = reader.ReadEvent()) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (record)
                {
                    var eventType = record.Level switch
                    {
                        2 => "Error",       // StandardEventLevel.Error
                        3 => "Warning",     // StandardEventLevel.Warning
                        _ => "Information"
                    };

                    entries.Add(new LogEventData
                    {
                        TimeGenerated = record.TimeCreated?.ToLocalTime() ?? DateTime.MinValue,
                        SourceName = record.ProviderName ?? string.Empty,
                        Message = GetEventMessage(record),
                        EventType = eventType,
                        LogFile = logName,
                        EventId = (uint)(record.Id >= 0 ? record.Id : 0)
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _log.LogError($"读取事件日志 '{logName}' 失败", ex);
        }

        return entries;
    }

    private static string GetEventMessage(EventRecord record)
    {
        try
        {
            return record.FormatDescription() ?? string.Empty;
        }
        catch
        {
            // FormatDescription may fail if the provider DLL is not available
            return string.Empty;
        }
    }
}
