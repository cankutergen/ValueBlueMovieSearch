using Identity.Business.Concrete;
using Identity.Business.Test.Base;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace Identity.Business.Test.Services
{
    public class PasswordServiceTests : TestBase
    {
        private readonly IConfiguration configuration;
        private readonly PasswordService passwordService;

        public PasswordServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>()
            {
                {"PasswordSettings:SaltSize", "16" },
                {"PasswordSettings:KeySize", "32" },
                {"PasswordSettings:Iterations", "1000" },
            };

            configuration = GetConfiguration(inMemorySettings);
            passwordService = new PasswordService(configuration);
        }

        [Fact]
        public void HashPassword_GivenPassword_ShouldChangePassword()
        {
            var password = "1234";

            var hashedPassword = passwordService.HashPassword(password);
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_GivenPasswordAndHashedPassword_ShouldReturnTrue()
        {
            var password = "1234";
            var hashedPassword = passwordService.HashPassword(password);

            var result = passwordService.VerifyPassword(password, hashedPassword);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_GivenSamePasswords_ShouldReturnFalse()
        {
            var hashedPassword = passwordService.HashPassword("1234");

            var result = passwordService.VerifyPassword("1111", hashedPassword);
            Assert.False(result);
        }
    }
}
