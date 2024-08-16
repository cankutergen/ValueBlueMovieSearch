using Identity.Business.Concrete;
using Identity.Business.Test.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Test.Services
{
    public class JwtTokenServiceTests : TestBase
    {
        private readonly IConfiguration configuration;
        private readonly JwtTokenService jwtTokenService;

        public JwtTokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>()
            {
                {"JwtTokenSettings:Key", "93b73f1d-4026-4630-a220-943142e4e9d2" },
                {"JwtTokenSettings:Issuer", "IdentityService" },
                {"JwtTokenSettings:ExpiryMinutes", "180" },
            };

            configuration = GetConfiguration(inMemorySettings);
            jwtTokenService = new JwtTokenService(configuration);
        }

        [Fact]
        public void GetToken_GivenUsernameAndRole_ShouldCreateToken()
        {
            var token = jwtTokenService.GetToken("user", "admin");

            Assert.NotNull(token.ExpirationTime);
            Assert.NotNull(token.Token);
        }
    }
}
