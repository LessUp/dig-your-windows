using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Disk information.
/// </summary>
public record DiskInfoData
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("fileSystem")]
    public string FileSystem { get; init; } = string.Empty;

    [JsonPropertyName("totalSpace")]
    public ulong TotalSpace { get; init; }

    [JsonPropertyName("availableSpace")]
    public ulong AvailableSpace { get; init; }

    [JsonIgnore]
    public long TotalSizeGB => (long)(TotalSpace / (1024UL * 1024UL * 1024UL));

    [JsonIgnore]
    public long FreeSpaceGB => (long)(AvailableSpace / (1024UL * 1024UL * 1024UL));

    [JsonIgnore]
    public double UsagePercentage => TotalSpace > 0
        ? (double)(TotalSpace - AvailableSpace) / TotalSpace * 100
        : 0;
}

/// <summary>
/// Disk SMART data.
/// </summary>
public record DiskSmartData
{
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; init; } = string.Empty;

    [JsonPropertyName("friendlyName")]
    public string FriendlyName { get; init; } = string.Empty;

    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; init; }

    [JsonPropertyName("busType")]
    public ushort? BusType { get; init; }

    [JsonPropertyName("mediaType")]
    public ushort? MediaType { get; init; }

    [JsonPropertyName("size")]
    public ulong Size { get; init; }

    [JsonPropertyName("healthStatus")]
    public ushort? HealthStatus { get; init; }

    [JsonPropertyName("temperature")]
    public ushort? Temperature { get; init; }

    [JsonPropertyName("wear")]
    public ushort? Wear { get; init; }

    [JsonPropertyName("powerOnHours")]
    public uint? PowerOnHours { get; init; }

    [JsonIgnore]
    public long SizeGB => (long)(Size / (1024UL * 1024UL * 1024UL));

    [JsonIgnore]
    public string HealthStatusDescription => HealthStatus switch
    {
        1 => "健康",
        2 => "警告",
        3 => "故障",
        _ => "未知"
    };

    [JsonIgnore]
    public string MediaTypeDescription => MediaType switch
    {
        3 => "HDD",
        4 => "SSD",
        5 => "SCM",
        _ => "未知"
    };

    [JsonIgnore]
    public string BusTypeDescription => BusType switch
    {
        1 => "SCSI",
        2 => "ATAPI",
        3 => "ATA",
        4 => "IEEE 1394",
        6 => "光纤通道",
        7 => "USB",
        8 => "RAID",
        9 => "iSCSI",
        10 => "SAS",
        11 => "SATA",
        12 => "SD",
        13 => "MMC",
        14 => "虚拟",
        16 => "存储空间",
        17 => "NVMe",
        _ => "未知"
    };
}
