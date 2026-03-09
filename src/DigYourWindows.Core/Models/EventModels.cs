using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Windows reliability record.
/// </summary>
public record ReliabilityRecordData
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    [JsonPropertyName("sourceName")]
    public string SourceName { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("eventType")]
    public string EventType { get; init; } = string.Empty;

    [JsonPropertyName("recordType")]
    public int? RecordType { get; init; }

    [JsonIgnore]
    public DateTime TimeGenerated => Timestamp;

    [JsonIgnore]
    public string ProductName => SourceName;

    [JsonIgnore]
    public string RecordTypeDescription => RecordType switch
    {
        1 => "应用程序故障",
        2 => "Windows 故障",
        3 => "其他故障",
        null => string.IsNullOrWhiteSpace(EventType) ? "未知" : EventType,
        _ => "未知"
    };
}

/// <summary>
/// Windows event log entry.
/// </summary>
public record LogEventData
{
    [JsonPropertyName("timeGenerated")]
    public DateTime TimeGenerated { get; init; }

    [JsonPropertyName("logFile")]
    public string LogFile { get; init; } = string.Empty;

    [JsonPropertyName("sourceName")]
    public string SourceName { get; init; } = string.Empty;

    [JsonIgnore]
    public string Source => SourceName;

    [JsonPropertyName("eventType")]
    public string EventType { get; init; } = string.Empty;

    [JsonPropertyName("eventId")]
    public uint EventId { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonIgnore]
    public string LogName => LogFile;
}
