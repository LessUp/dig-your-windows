using System.Management;
using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface IHardwareService
{
    HardwareData GetHardwareInfo(CancellationToken cancellationToken = default);
}

public class HardwareService : IHardwareService
{
    private readonly IGpuMonitorService _gpuMonitor;
    private readonly IDiskSmartService _diskSmartService;
    private readonly ILogService _log;

    public HardwareService(IGpuMonitorService gpuMonitor, IDiskSmartService diskSmartService, ILogService log)
    {
        _gpuMonitor = gpuMonitor;
        _diskSmartService = diskSmartService;
        _log = log;
    }

    public HardwareData GetHardwareInfo(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cpuBrand = GetCpuName();
        cancellationToken.ThrowIfCancellationRequested();
        var totalMemory = GetTotalMemoryBytes();
        cancellationToken.ThrowIfCancellationRequested();
        var disks = GetDisks();
        cancellationToken.ThrowIfCancellationRequested();
        var diskSmart = _diskSmartService.GetDiskSmart();
        cancellationToken.ThrowIfCancellationRequested();
        var networkAdapters = GetNetworkAdapters();
        cancellationToken.ThrowIfCancellationRequested();
        var usbDevices = GetUsbDevices();
        cancellationToken.ThrowIfCancellationRequested();
        var usbControllers = GetUsbControllers();
        cancellationToken.ThrowIfCancellationRequested();

        return new HardwareData
        {
            ComputerName = Environment.MachineName,
            OsVersion = Environment.OSVersion.ToString(),
            CpuBrand = cpuBrand,
            CpuCores = (uint)Environment.ProcessorCount,
            TotalMemory = totalMemory,
            Disks = disks,
            DiskSmart = diskSmart,
            NetworkAdapters = networkAdapters,
            UsbDevices = usbDevices,
            UsbControllers = usbControllers,
            Gpus = _gpuMonitor.GetGpuInfo()
        };
    }

    private string GetCpuName()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                using (obj)
                {
                    return obj["Name"]?.ToString()?.Trim() ?? "Unknown";
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取CPU名称失败: {ex.Message}");
        }
        return "Unknown CPU";
    }

    private ulong GetTotalMemoryBytes()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
            {
                using (obj)
                {
                    return Convert.ToUInt64(obj["TotalPhysicalMemory"] ?? 0UL);
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取总内存失败: {ex.Message}");
        }
        return 0UL;
    }

    private List<DiskInfoData> GetDisks()
    {
        var disks = new List<DiskInfoData>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT Name, FileSystem, Size, FreeSpace FROM Win32_LogicalDisk WHERE DriveType = 3");
            foreach (ManagementObject obj in searcher.Get())
            {
                using (obj)
                {
                    var size = Convert.ToUInt64(obj["Size"] ?? 0UL);
                    var freeSpace = Convert.ToUInt64(obj["FreeSpace"] ?? 0UL);

                    disks.Add(new DiskInfoData
                    {
                        Name = obj["Name"]?.ToString() ?? "",
                        FileSystem = obj["FileSystem"]?.ToString() ?? "",
                        TotalSpace = size,
                        AvailableSpace = freeSpace
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取磁盘信息失败: {ex.Message}");
        }
        return disks;
    }

    private List<NetworkAdapterData> GetNetworkAdapters()
    {
        var adapters = new List<NetworkAdapterData>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");

            foreach (ManagementObject obj in searcher.Get())
            {
                using (obj)
                {
                    var ipAddresses = obj["IPAddress"] as string[];
                    adapters.Add(new NetworkAdapterData
                    {
                        Name = obj["Description"]?.ToString() ?? "",
                        MacAddress = obj["MACAddress"]?.ToString() ?? "",
                        IpAddresses = ipAddresses?.ToList() ?? new List<string>()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取网络适配器失败: {ex.Message}");
        }
        return adapters;
    }

    private List<UsbDeviceData> GetUsbDevices()
    {
        var devices = new List<UsbDeviceData>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT DeviceID, Name, Description, Manufacturer FROM Win32_PnPEntity WHERE DeviceID LIKE 'USB%'");

            foreach (ManagementObject obj in searcher.Get())
            {
                using (obj)
                {
                    devices.Add(new UsbDeviceData
                    {
                        DeviceId = obj["DeviceID"]?.ToString() ?? "",
                        Name = obj["Name"]?.ToString(),
                        Description = obj["Description"]?.ToString(),
                        Manufacturer = obj["Manufacturer"]?.ToString()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取USB设备失败: {ex.Message}");
        }
        return devices;
    }

    private List<UsbControllerData> GetUsbControllers()
    {
        var controllers = new List<UsbControllerData>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT DeviceID, Name, Manufacturer, Caption FROM Win32_USBController");

            foreach (ManagementObject obj in searcher.Get())
            {
                using (obj)
                {
                    var caption = obj["Caption"]?.ToString() ?? "";
                    var protocol = caption.Contains("3.0") || caption.Contains("3.1") || caption.Contains("xHCI") 
                        ? "USB 3.x" 
                        : "USB 2.0";

                    var deviceId = obj["DeviceID"]?.ToString();
                    var name = obj["Name"]?.ToString();

                    controllers.Add(new UsbControllerData
                    {
                        DeviceId = string.IsNullOrWhiteSpace(deviceId) ? (name ?? string.Empty) : deviceId,
                        Name = name,
                        Manufacturer = obj["Manufacturer"]?.ToString(),
                        Caption = caption,
                        ProtocolVersion = protocol
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"获取USB控制器失败: {ex.Message}");
        }
        return controllers;
    }
}
