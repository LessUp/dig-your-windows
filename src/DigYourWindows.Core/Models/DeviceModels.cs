using System.Text.Json.Serialization;

namespace DigYourWindows.Core.Models;

/// <summary>
/// Network adapter information.
/// </summary>
public record NetworkAdapterData
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("macAddress")]
    public string MacAddress { get; init; } = string.Empty;

    [JsonPropertyName("ipAddresses")]
    public List<string> IpAddresses { get; init; } = new();

    [JsonIgnore]
    public string IpAddress => IpAddresses.Count > 0 ? IpAddresses[0] : string.Empty;
}

/// <summary>
/// USB device information.
/// </summary>
public record UsbDeviceData
{
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; init; }

    [JsonPropertyName("pnpDeviceId")]
    public string? PnpDeviceId { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>
/// USB controller information.
/// </summary>
public record UsbControllerData
{
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; init; }

    [JsonPropertyName("caption")]
    public string? Caption { get; init; }

    [JsonPropertyName("protocolVersion")]
    public string? ProtocolVersion { get; init; }

    [JsonIgnore]
    public string Protocol => ProtocolVersion ?? string.Empty;
}
