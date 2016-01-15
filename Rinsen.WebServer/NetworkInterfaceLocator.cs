using System;
using Microsoft.SPOT;
using System.Net;
using Microsoft.SPOT.Net.NetworkInformation;
using Rinsen.WebServer.Extensions;

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
                Debug.Print("Physical address: " + networkInterface.PhysicalAddress.ToHexString());

                Debug.Print("Subnet mask: " + networkInterface.SubnetMask);
                count++;
            }
#endif
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.GatewayAddress != "0.0.0.0")
                {
                    Debug.Print("Selected interface: " + networkInterface.IPAddress);
                    return IPAddress.Parse(networkInterface.IPAddress);
                }
            }
            throw new Exception("No network interface detected");
        }

    }
}
