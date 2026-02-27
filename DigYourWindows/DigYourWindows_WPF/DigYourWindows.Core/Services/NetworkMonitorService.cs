using System.Net.NetworkInformation;

namespace DigYourWindows.Core.Services;

public interface INetworkMonitorService
{
    (long BytesReceived, long BytesSent) GetTotalBytes();
}

public sealed class NetworkMonitorService : INetworkMonitorService
{
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
            catch
            {
            }
        }

        return (received, sent);
    }
}
