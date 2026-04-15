using System.Net.NetworkInformation;

namespace DigYourWindows.Core.Services;

public interface INetworkMonitorService
{
    (long BytesReceived, long BytesSent) GetTotalBytes();
}

public sealed class NetworkMonitorService : INetworkMonitorService
{
    private readonly ILogService _log;

    public NetworkMonitorService(ILogService log)
    {
        _log = log;
    }

    public (long BytesReceived, long BytesSent) GetTotalBytes()
    {
        long received = 0;
        long sent = 0;

        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus != OperationalStatus.Up)
            {
                continue;
            }

            if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
            {
                continue;
            }

            try
            {
                var stats = nic.GetIPv4Statistics();
                received += stats.BytesReceived;
                sent += stats.BytesSent;
            }
            catch (Exception ex)
            {
                _log.Warn($"获取网络接口 {nic.Name} 统计信息失败: {ex.Message}");
            }
        }

        return (received, sent);
    }
}
