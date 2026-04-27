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
    private readonly object _lock = new();
    private Computer? _computer;
    private bool _disposed;

    public Computer Computer
    {
        get
        {
            lock (_lock)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return _computer ?? throw new ObjectDisposedException(nameof(HardwareMonitorProvider));
            }
        }
    }

    public HardwareMonitorProvider()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true
        };
        try
        {
            _computer.Open();
        }
        catch
        {
            _computer.Close();
            throw;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _computer?.Close();
            _computer = null;
        }
    }
}
