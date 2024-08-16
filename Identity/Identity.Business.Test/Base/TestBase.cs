using Identity.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Test.Base
{
    public class TestBase
    {
        protected UserDocument TestModel;

        public TestBase()
        {
            TestModel = new UserDocument();
        }

        public UserDocument GetValidModel()
        {
            return new UserDocument
            {
                Id = ObjectId.GenerateNewId(),
                Password = "11223344",
                Username = "Test",
                Role = new UserRole
                {
                    Id = ObjectId.GenerateNewId(),
                    RoleName = "TEST"
                }
            };
        }

        public IConfiguration GetConfiguration(Dictionary<string, string> inMemorySettings)
        {
            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }
    }
}
