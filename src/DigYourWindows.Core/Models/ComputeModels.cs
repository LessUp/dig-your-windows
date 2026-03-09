using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// GPU information.
/// </summary>
public record GpuInfoData
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("driverVersion")]
    public string? DriverVersion { get; init; }

    [JsonPropertyName("videoMemory")]
    public ulong? VideoMemory { get; init; }

    [JsonPropertyName("temperature")]
    public float Temperature { get; init; }

    [JsonPropertyName("load")]
    public float Load { get; init; }

    [JsonPropertyName("memoryUsed")]
    public float MemoryUsed { get; init; }

    [JsonPropertyName("memoryTotal")]
    public float MemoryTotal { get; init; }

    [JsonPropertyName("coreClock")]
    public float CoreClock { get; init; }

    [JsonPropertyName("memoryClock")]
    public float MemoryClock { get; init; }

    [JsonPropertyName("fanSpeed")]
    public float FanSpeed { get; init; }

    [JsonPropertyName("power")]
    public float Power { get; init; }
}

/// <summary>
/// CPU information (real-time monitoring data).
/// </summary>
public record CpuInfoData
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("temperature")]
    public float Temperature { get; init; }

    [JsonPropertyName("load")]
    public float Load { get; init; }

    [JsonPropertyName("clock")]
    public float Clock { get; init; }
}
