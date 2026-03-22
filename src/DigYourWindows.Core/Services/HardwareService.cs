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
        return ExecuteSafely(
            operationName: "获取CPU名称",
            fallback: "Unknown CPU",
            action: () =>
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                {
                    using (obj)
                    {
                        return obj["Name"]?.ToString()?.Trim() ?? "Unknown";
                    }
                }

                return "Unknown CPU";
            });
    }

    private ulong GetTotalMemoryBytes()
    {
        return ExecuteSafely(
            operationName: "获取总内存",
            fallback: 0UL,
            action: () =>
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                foreach (var obj in searcher.Get())
                {
                    using (obj)
                    {
                        return Convert.ToUInt64(obj["TotalPhysicalMemory"] ?? 0UL, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                return 0UL;
            });
    }

    private List<DiskInfoData> GetDisks()
    {
        return ExecuteSafely(
            operationName: "获取磁盘信息",
            fallback: new List<DiskInfoData>(),
            action: () =>
            {
                var disks = new List<DiskInfoData>();
                using var searcher = new ManagementObjectSearcher(
                    "SELECT Name, FileSystem, Size, FreeSpace FROM Win32_LogicalDisk WHERE DriveType = 3");
                foreach (ManagementObject obj in searcher.Get())
                {
                    using (obj)
                    {
                        var size = Convert.ToUInt64(obj["Size"] ?? 0UL, System.Globalization.CultureInfo.InvariantCulture);
                        var freeSpace = Convert.ToUInt64(obj["FreeSpace"] ?? 0UL, System.Globalization.CultureInfo.InvariantCulture);

                        disks.Add(new DiskInfoData
                        {
                            Name = obj["Name"]?.ToString() ?? "",
                            FileSystem = obj["FileSystem"]?.ToString() ?? "",
                            TotalSpace = size,
                            AvailableSpace = freeSpace
                        });
                    }
                }

                return disks;
            });
    }

    private List<NetworkAdapterData> GetNetworkAdapters()
    {
        return ExecuteSafely(
            operationName: "获取网络适配器",
            fallback: new List<NetworkAdapterData>(),
            action: () =>
            {
                var adapters = new List<NetworkAdapterData>();
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

                return adapters;
            });
    }

    private List<UsbDeviceData> GetUsbDevices()
    {
        return ExecuteSafely(
            operationName: "获取USB设备",
            fallback: new List<UsbDeviceData>(),
            action: () =>
            {
                var devices = new List<UsbDeviceData>();
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

                return devices;
            });
    }

    private List<UsbControllerData> GetUsbControllers()
    {
        return ExecuteSafely(
            operationName: "获取USB控制器",
            fallback: new List<UsbControllerData>(),
            action: () =>
            {
                var controllers = new List<UsbControllerData>();
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

                return controllers;
            });
    }

    private T ExecuteSafely<T>(string operationName, T fallback, Func<T> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            _log.Warn($"{operationName}失败: {ex.Message}");
            return fallback;
        }
    }
}
