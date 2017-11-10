using System;
using Microsoft.SPOT;
using System.Net;
using Microsoft.SPOT.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Rinsen.WebServer
{
    internal class NetworkInterfaceLocator
    {
        public IPAddress Locate()
        {
#if DEBUG
            var count = 1;
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.Print("Interface nr: " + count);
                Debug.Print("Is dhcp enabled: " + networkInterface.IsDhcpEnabled);
                Debug.Print("Interface address: " + networkInterface.IPAddress);
                Debug.Print("Gateway address: " + networkInterface.GatewayAddress);
                foreach (var dnsAddress in networkInterface.DnsAddresses)
                {
                    Debug.Print("Dns address: " + dnsAddress);
                }
                var sb = new StringBuilder("Physical address: ");
                var parts = 1;
                foreach (var physicalAddress in networkInterface.PhysicalAddress)
                {
                    sb.Append(physicalAddress);
                    if (parts < networkInterface.PhysicalAddress.Length)
                        sb.Append("-");
                    parts++;
                }
                Debug.Print(sb.ToString());

                Debug.Print("Subnet mask: " + networkInterface.SubnetMask);
                count++;
            }
#endif
            // add retry logic to give the device a chance to init network, most likely needed wireless
            int retryCount = 0;
            int maxRetries = 10;
            while (true)
            {
                if (retryCount >= maxRetries) break;

                Debug.Print("Getting IP address...");
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.GatewayAddress != "0.0.0.0")
                    {
                        Debug.Print("Selected interface: " + networkInterface.IPAddress);
                        return IPAddress.Parse(networkInterface.IPAddress);
                    }
                }
                Thread.Sleep(1000);
                retryCount++;
            }
            throw new Exception("No network interface detected");
        }
    }
}
