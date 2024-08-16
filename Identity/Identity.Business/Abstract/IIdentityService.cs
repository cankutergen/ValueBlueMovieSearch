using Identity.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Abstract
{
    public interface IIdentityService
    {
        Task<UserDocument> GetUserByUsernameAndPasswordAsync(string username, string password);

        Task RegisterUserAsync(UserDocument user);

        Task<bool> IsUserExists(string username);
    }
}
