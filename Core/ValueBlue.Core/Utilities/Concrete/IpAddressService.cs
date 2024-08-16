using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Utilities.Abstract;


namespace ValueBlue.Core.Utilities.Concrete
{
    public class IpAddressService : IIpAddressService
    {
        public string GetIpAddress(IPAddress remoteIpAddress)
        {
            string ipAddress = "0.0.0.0";
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                ipAddress = remoteIpAddress.ToString();
            }

            return ipAddress;
        }
    }
}
