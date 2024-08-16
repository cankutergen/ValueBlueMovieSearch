using FluentValidation;
using Identity.Business.Abstract;
using Identity.Business.Concrete;
using Identity.Business.Test.Base;
using Identity.DataAccess.Abstract;
using Identity.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.Test.Services 
{
    public class IdentityServiceTests : TestBase
    {
        private readonly Mock<IIdentityRepository> repositoryMock;
        private readonly Mock<IPasswordService> passwordServiceMock;
        private readonly Mock<IValidator<UserDocument>> validatorMock;
        private readonly IdentityService service;

        public IdentityServiceTests()
        {
            repositoryMock = new Mock<IIdentityRepository>();
            passwordServiceMock = new Mock<IPasswordService>();
            validatorMock = new Mock<IValidator<UserDocument>>();

            service = new IdentityService(repositoryMock.Object, passwordServiceMock.Object, validatorMock.Object);  
        }

        [Fact]
        public void GetUserByUsernameAndPasswordAsync_GivenInvalidUser_ShouldReturnAuthenticationException()
        {
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserDocument, bool>>>()));

            Task result() => service.GetUserByUsernameAndPasswordAsync("", "");

            Assert.ThrowsAsync<AuthenticationException>(result);
        }

        [Fact]
        public void GetUserByUsernameAndPasswordAsync_GivenInvalidPassword_ShouldReturnAuthenticationException()
        {
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserDocument, bool>>>()))
                .Returns(Task.FromResult(new UserDocument()));

            passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            Task result() => service.GetUserByUsernameAndPasswordAsync("", "");

            Assert.ThrowsAsync<AuthenticationException>(result);
        }

        [Fact]
        public async Task GetUserByUsernameAndPasswordAsync_GivenValidCredentials_ShouldReturnUser()
        {
            var model = GetValidModel();

            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserDocument, bool>>>()))
                .Returns(Task.FromResult(model));

            passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var result = await service.GetUserByUsernameAndPasswordAsync("username", "");

            Assert.NotNull(result);
        }

        [Fact]
        public void RegisterUserAsync_GivenInvalidModel_ShouldThrowValidationException()
        {
            validatorMock.Setup(x => x.Validate(It.IsAny<UserDocument>())).Throws(new ValidationException(""));

            Task result() => service.RegisterUserAsync(new UserDocument());

            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task IsUserExists_GivenInvalidUser_ShouldReturnFalse()
        {
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserDocument, bool>>>()));

            var result = await service.IsUserExists("username");
            Assert.False(result);
        }

        [Fact]
        public async Task IsUserExists_GivenValidUser_ShouldReturnTrue()
        {
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserDocument, bool>>>()))
                .Returns(Task.FromResult(new UserDocument()));

            var result = await service.IsUserExists("username");
            Assert.True(result);
        }
    }
}
