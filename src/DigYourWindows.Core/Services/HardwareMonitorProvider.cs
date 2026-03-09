using LibreHardwareMonitor.Hardware;

namespace DigYourWindows.Core.Services;

/// <summary>
/// Shared LibreHardwareMonitor Computer instance.
/// Avoids creating multiple heavyweight Computer objects for CPU/GPU monitoring.
/// </summary>
public interface IHardwareMonitorProvider : IDisposable
{
    Computer Computer { get; }
}

public sealed class HardwareMonitorProvider : IHardwareMonitorProvider
{
    public Computer Computer { get; }

    public HardwareMonitorProvider()
    {
        Computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true
        };
        Computer.Open();
    }

    public void Dispose()
    {
        Computer.Close();
    }
}
