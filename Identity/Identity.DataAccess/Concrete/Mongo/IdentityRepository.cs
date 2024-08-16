using Identity.DataAccess.Abstract;
using Identity.Entities.Concrete;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.DataAccess.Mongo.Concrete;

namespace Identity.DataAccess.Concrete.Mongo
{
    public class IdentityRepository : MongoEntityRepositoryBase<UserDocument>, IIdentityRepository
    {
        public IdentityRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
