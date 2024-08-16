using Identity.Business.Abstract;
using Identity.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace Identity.Business.Concrete
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtTokenSettings jwtTokenSettings;

        public JwtTokenService(IConfiguration configuration)
        {
            jwtTokenSettings = configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();
        }

        public AccessToken GetToken(string username, string role)
        {
            var key = Encoding.ASCII.GetBytes(jwtTokenSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtTokenSettings.ExpiryMinutes.ToString())),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtTokenSettings.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationTime = tokenDescriptor.Expires
            };
        }
    }
}
