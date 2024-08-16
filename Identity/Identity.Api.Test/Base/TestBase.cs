using Identity.Api.Models;
using Identity.Entities.Concrete;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Test.Base
{
    public class TestBase
    {
        protected AccessTokenRequest TestAccessTokenRequest;
        protected AccessToken TestAccessToken;
        protected UserDocument TestUserDocument;

        public TestBase()
        {
            TestAccessTokenRequest = new AccessTokenRequest { Password = "password", Username = "username" };
            TestAccessToken = new AccessToken { ExpirationTime = DateTime.UtcNow, Token = "" };
            TestUserDocument = new UserDocument
            {
                Id = new ObjectId(),
                Password = "password",
                Role = new UserRole 
                {
                    Id = new ObjectId(),
                    RoleName = "admin"
                },
                Username = "username"
            };
        }
    }
}
