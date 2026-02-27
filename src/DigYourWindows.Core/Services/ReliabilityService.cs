using System.Management;
using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface IReliabilityService
{
    ReliabilityRecordData[] GetReliabilityRecords(int daysBack = 7);
}

public class ReliabilityService : IReliabilityService
{
    private readonly ILogService _log;

    public ReliabilityService(ILogService log)
    {
        _log = log;
    }

    public ReliabilityRecordData[] GetReliabilityRecords(int daysBack = 7)
    {
        var records = new List<ReliabilityRecordData>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT TimeGenerated, ProductName, Message, RecordType FROM Win32_ReliabilityRecords");
            
            var cutoffDate = DateTime.Now.AddDays(-daysBack);
            
            foreach (ManagementObject obj in searcher.Get())
            {
                var timeStr = obj["TimeGenerated"]?.ToString();
                if (string.IsNullOrEmpty(timeStr) || timeStr.Length < 14)
                    continue;

                // WMI datetime format: yyyyMMddHHmmss.ffffff+UUU
                var timeGenerated = ParseWmiDateTime(timeStr);
                
                if (timeGenerated < cutoffDate)
                    continue;

                var recordType = Convert.ToInt32(obj["RecordType"] ?? 0);
                var recordTypeDescription = recordType switch
                {
                    1 => "应用程序故障",
                    2 => "Windows 故障",
                    3 => "其他故障",
                    _ => "未知"
                };

                records.Add(new ReliabilityRecordData
                {
                    Timestamp = timeGenerated,
                    SourceName = obj["ProductName"]?.ToString() ?? "",
                    Message = obj["Message"]?.ToString() ?? "",
                    EventType = recordTypeDescription,
                    RecordType = recordType
                });
            }
        }
        catch (Exception ex)
        {
            _log.Error("获取可靠性记录失败", ex);
        }
        return records.OrderByDescending(r => r.Timestamp).ToArray();
    }

    private static DateTime ParseWmiDateTime(string wmiDateTime)
    {
        try
        {
            return ManagementDateTimeConverter.ToDateTime(wmiDateTime);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
}
