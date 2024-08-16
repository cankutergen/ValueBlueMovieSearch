using Identity.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.DataAccess;

namespace Identity.DataAccess.Abstract
{
    public interface IIdentityRepository : IEntityRepository<UserDocument>
    {
    }
}
