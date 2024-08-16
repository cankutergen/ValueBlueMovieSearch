using FluentValidation;
using Identity.Business.Abstract;
using Identity.DataAccess.Abstract;
using Identity.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.CrossCuttingConcerns;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace Identity.Business.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository repository;
        private readonly IPasswordService passwordService;
        private readonly IValidator<UserDocument> validator;

        public IdentityService(IIdentityRepository repository, IPasswordService passwordService, IValidator<UserDocument> validator)
        {
            this.repository = repository;
            this.passwordService = passwordService;
            this.validator = validator;
        }

        public async Task<UserDocument> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            var user = await GetUserByUserNameAsync(username);

            if (user == null)
            {
                throw new AuthenticationException("User does not exist");
            }

            if (!passwordService.VerifyPassword(password.Trim(), user.Password)) 
            {
                throw new AuthenticationException("Password does not match");
            }

            return user;
        }

        public async Task RegisterUserAsync(UserDocument user)
        {
            if(user.Id == null)
            {
                user.Id = ObjectId.GenerateNewId();
            }

            if (user.Role.Id == null)
            {

                user.Role.Id = ObjectId.GenerateNewId();
            }

            ValidatorTool.FluentValidate(validator, user);

            var hashedPassword = passwordService.HashPassword(user.Password.Trim());

            user.Password = hashedPassword;
            user.Username = user.Username.ToUpper();
            user.Role.RoleName = user.Role.RoleName.ToUpper();

            await repository.AddAsync(user);
        }

        public async Task<bool> IsUserExists(string username)
        {
            var user = await GetUserByUserNameAsync(username);
            return user != null;
        }

        private async Task<UserDocument> GetUserByUserNameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username is not given");
            }

            return await repository.GetAsync(x => x.Username.ToUpper() == username.Trim().ToUpper());
        }
    }
}
