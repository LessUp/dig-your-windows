using DigYourWindows.Core.Models;
using LibreHardwareMonitor.Hardware;

namespace DigYourWindows.Core.Services;

public interface IGpuMonitorService
{
    List<GpuInfoData> GetGpuInfo();
}

public class GpuMonitorService : IGpuMonitorService
{
    private readonly IHardwareMonitorProvider _provider;
    private readonly ILogService _log;

    public GpuMonitorService(IHardwareMonitorProvider provider, ILogService log)
    {
        _provider = provider;
        _log = log;
    }

    public List<GpuInfoData> GetGpuInfo()
    {
        var gpuList = new List<GpuInfoData>();

        try
        {
            foreach (var hardware in _provider.Computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuNvidia ||
                    hardware.HardwareType == HardwareType.GpuAmd ||
                    hardware.HardwareType == HardwareType.GpuIntel)
                {
                    hardware.Update();

                    var gpuInfo = new GpuInfoData
                    {
                        Name = hardware.Name,
                        DriverVersion = null,
                        VideoMemory = null,
                        Temperature = GetSensorValue(hardware, SensorType.Temperature, "GPU Core"),
                        Load = GetSensorValue(hardware, SensorType.Load, "GPU Core"),
                        MemoryUsed = GetSensorValue(hardware, SensorType.SmallData, "GPU Memory Used"),
                        MemoryTotal = GetSensorValue(hardware, SensorType.SmallData, "GPU Memory Total"),
                        CoreClock = GetSensorValue(hardware, SensorType.Clock, "GPU Core"),
                        MemoryClock = GetSensorValue(hardware, SensorType.Clock, "GPU Memory"),
                        FanSpeed = GetSensorValue(hardware, SensorType.Control, "GPU Fan"),
                        Power = GetSensorValue(hardware, SensorType.Power, "GPU Package")
                    };

                    gpuList.Add(gpuInfo);
                }
            }
        }
        catch (Exception ex)
        {
            _log.Error("获取GPU信息失败", ex);
        }

        return gpuList;
    }

    private float GetSensorValue(IHardware hardware, SensorType sensorType, string nameFilter)
    {
        try
        {
            var sensor = hardware.Sensors
                .FirstOrDefault(s => s.SensorType == sensorType && 
                                    (string.IsNullOrEmpty(nameFilter) || s.Name.Contains(nameFilter)));

            return sensor?.Value ?? 0f;
        }
        catch (Exception ex)
        {
            _log.Warn($"读取GPU传感器失败: {ex.Message}");
            return 0f;
        }
    }

}
