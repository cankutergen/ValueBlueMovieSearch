using Identity.Business.Test.Base;
using Identity.Business.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Test.ValidationRules
{
    public class UserValidatorTests : TestBase
    {
        private readonly UserValidator validationRules;

        public UserValidatorTests()
        {
            validationRules = new UserValidator();
            TestModel = GetValidModel();
        }

        [Fact]
        public void Validate_GivenEmptyUsername_ShouldReturnErrorList()
        {
            TestModel.Username = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenNullUserId_ShouldReturnErrorList()
        {
            TestModel.Id = null;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenEmptyPassword_ShouldReturnErrorList()
        {
            TestModel.Password = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenEmptyRoleName_ShouldReturnErrorList()
        {
            TestModel.Role.RoleName = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }


        [Fact]
        public void Validate_GivenNullRoleId_ShouldReturnErrorList()
        {
            TestModel.Role.Id = null;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }
    }
}
