using AutoMapper;
using FluentValidation;
using Identity.Api.Controllers;
using Identity.Api.Models;
using Identity.Api.Test.Base;
using Identity.Business.Abstract;
using Identity.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Test.Controllers
{
    public class IdentityControllerTests : TestBase
    {
        private readonly Mock<IIdentityService> identityServiceMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ILogger<IdentityController>> loggerMock;
        private readonly Mock<ITokenService> tokenServiceMock;

        private readonly IdentityController controller;

        public IdentityControllerTests()
        {
            identityServiceMock = new Mock<IIdentityService>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<IdentityController>>();
            tokenServiceMock = new Mock<ITokenService>();

            controller = new IdentityController(identityServiceMock.Object, mapperMock.Object, loggerMock.Object, tokenServiceMock.Object);
        }

        [Fact]
        public async Task GenerateAccessToken_GivenValidCredentials_ShouldReturnOkResponse()
        {
            identityServiceMock.Setup(x => x.GetUserByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(TestUserDocument));

            tokenServiceMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(TestAccessToken);

            var result = await controller.GenerateAccessToken(TestAccessTokenRequest);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GenerateAccessToken_ThrownAuthenticationException_ShouldReturnUnauthorizedResponse()
        {
            identityServiceMock.Setup(x => x.GetUserByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new AuthenticationException());

            var result = await controller.GenerateAccessToken(TestAccessTokenRequest);
            Assert.IsAssignableFrom<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GenerateAccessToken_ThrownException_ShouldReturn500StatusCode()
        {
            identityServiceMock.Setup(x => x.GetUserByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            var result = await controller.GenerateAccessToken(TestAccessTokenRequest);
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_GivenExistingUser_ShouldReturn409StatusCode()
        {
            mapperMock.Setup(x => x.Map<UserDocument>(It.IsAny<RegisterRequest>()))
                .Returns(TestUserDocument);

            identityServiceMock.Setup(x => x.IsUserExists(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var result = await controller.Post(new RegisterRequest());
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(409, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_GivenNewUser_ShouldReturnCreatedResponse()
        {
            mapperMock.Setup(x => x.Map<UserDocument>(It.IsAny<RegisterRequest>()))
                .Returns(TestUserDocument);

            identityServiceMock.Setup(x => x.IsUserExists(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            identityServiceMock.Setup(x => x.RegisterUserAsync(It.IsAny<UserDocument>()));

            var result = await controller.Post(new RegisterRequest());
            Assert.IsAssignableFrom<CreatedResult>(result);
        }


        [Fact]
        public async Task Post_ThrownValidationException_ShouldReturn422StatusCode()
        {
            mapperMock.Setup(x => x.Map<UserDocument>(It.IsAny<RegisterRequest>()))
                .Returns(TestUserDocument);

            identityServiceMock.Setup(x => x.IsUserExists(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            identityServiceMock.Setup(x => x.RegisterUserAsync(It.IsAny<UserDocument>()))
                .Throws(new ValidationException(""));

            var result = await controller.Post(new RegisterRequest());
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(422, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_ThrownException_ShouldReturn422StatusCode()
        {
            mapperMock.Setup(x => x.Map<UserDocument>(It.IsAny<RegisterRequest>()))
                .Throws(new Exception());

            var result = await controller.Post(new RegisterRequest());
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_ThrownArgumentException_ShouldReturnBadRequest()
        {
            mapperMock.Setup(x => x.Map<UserDocument>(It.IsAny<RegisterRequest>()))
                .Throws(new ArgumentException());

            var result = await controller.Post(new RegisterRequest());
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }
    }
}
