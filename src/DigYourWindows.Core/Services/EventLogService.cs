using System.Diagnostics;
using DigYourWindows.Core.Models;
using SysEventLogEntry = System.Diagnostics.EventLogEntry;

namespace DigYourWindows.Core.Services;

public interface IEventLogService
{
    List<LogEventData> GetErrorEvents(int daysBack = 3);
}

public class EventLogService : IEventLogService
{
    private readonly ILogService _log;

    public EventLogService(ILogService log)
    {
        _log = log;
    }
    public List<LogEventData> GetErrorEvents(int daysBack = 3)
    {
        var events = new List<LogEventData>();
        var cutoffDate = DateTime.Now.AddDays(-daysBack);

        try
        {
            // System Log
            events.AddRange(ReadEventLog("System", cutoffDate));
            
            // Application Log
            events.AddRange(ReadEventLog("Application", cutoffDate));
        }
        catch (Exception ex)
        {
            _log.Error("读取事件日志失败", ex);
        }

        return events.OrderByDescending(e => e.TimeGenerated).ToList();
    }

    private List<LogEventData> ReadEventLog(string logName, DateTime cutoffDate)
    {
        var entries = new List<LogEventData>();
        
        try
        {
            using var eventLog = new EventLog(logName);
            
            foreach (SysEventLogEntry entry in eventLog.Entries)
            {
                if (entry.TimeGenerated < cutoffDate)
                    continue;

                // Only Error and Warning
                if (entry.EntryType != EventLogEntryType.Error && 
                    entry.EntryType != EventLogEntryType.Warning)
                    continue;

                entries.Add(new LogEventData
                {
                    TimeGenerated = entry.TimeGenerated,
                    SourceName = entry.Source,
                    Message = entry.Message,
                    EventType = entry.EntryType.ToString(),
                    LogFile = logName,
                    EventId = entry.InstanceId is >= 0 and <= uint.MaxValue
                        ? (uint)entry.InstanceId
                        : 0u
                });
            }
        }
        catch (Exception ex)
        {
            _log.Error($"读取事件日志 '{logName}' 失败", ex);
        }

        return entries;
    }
}
