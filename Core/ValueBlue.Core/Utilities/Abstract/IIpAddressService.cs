using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Core.Utilities.Abstract
{
    public interface IIpAddressService
    {
        string GetIpAddress(IPAddress remoteIpAddress);
    }
}
