using AutoMapper;
using FluentValidation;
using Identity.Api.Models;
using Identity.Business.Abstract;
using Identity.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;
        private readonly ILogger<IdentityController> logger;
        private readonly ITokenService tokenService;

        public IdentityController(IIdentityService identityService, IMapper mapper, ILogger<IdentityController> logger, ITokenService tokenService)
        {
            this.identityService = identityService;
            this.mapper = mapper;
            this.logger = logger;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("GenerateAccessToken")]
        public async Task<IActionResult> GenerateAccessToken([FromForm] AccessTokenRequest accessTokenRequest)
        {
            try
            {
                var user = await identityService.GetUserByUsernameAndPasswordAsync(accessTokenRequest.Username, accessTokenRequest.Password);
                return Ok(tokenService.GetToken(user.Username, user.Role.RoleName));

            }
            catch (AuthenticationException ex)
            {
                logger.LogError(ex.Message, ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("RegisterUser")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Post([FromForm] RegisterRequest model)
        {
            try
            {
                var userDocument = mapper.Map<UserDocument>(model);

                if(await identityService.IsUserExists(userDocument.Username))
                {
                    return StatusCode(409, $"User {userDocument.Username} already exists");
                }

                await identityService.RegisterUserAsync(userDocument);
                return Created();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
