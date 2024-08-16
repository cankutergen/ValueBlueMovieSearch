using Identity.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Abstract
{
    public interface ITokenService
    {
        AccessToken GetToken(string username, string role);
    }
}
