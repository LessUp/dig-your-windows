using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Hardware information collected from the system.
/// </summary>
public record HardwareData
{
    [JsonPropertyName("computerName")]
    public string ComputerName { get; init; } = string.Empty;

    [JsonPropertyName("osVersion")]
    public string OsVersion { get; init; } = string.Empty;

    [JsonPropertyName("cpuBrand")]
    public string CpuBrand { get; init; } = string.Empty;

    [JsonIgnore]
    public string CpuName => CpuBrand;

    [JsonPropertyName("cpuCores")]
    public uint CpuCores { get; init; }

    [JsonPropertyName("totalMemory")]
    public ulong TotalMemory { get; init; }

    [JsonIgnore]
    public long TotalMemoryMB => (long)(TotalMemory / (1024UL * 1024UL));

    [JsonPropertyName("disks")]
    public List<DiskInfoData> Disks { get; init; } = new();

    [JsonPropertyName("diskSmart")]
    public List<DiskSmartData> DiskSmart { get; init; } = new();

    [JsonPropertyName("networkAdapters")]
    public List<NetworkAdapterData> NetworkAdapters { get; init; } = new();

    [JsonPropertyName("usbDevices")]
    public List<UsbDeviceData> UsbDevices { get; init; } = new();

    [JsonPropertyName("usbControllers")]
    public List<UsbControllerData> UsbControllers { get; init; } = new();

    [JsonPropertyName("gpus")]
    public List<GpuInfoData> Gpus { get; init; } = new();
}
