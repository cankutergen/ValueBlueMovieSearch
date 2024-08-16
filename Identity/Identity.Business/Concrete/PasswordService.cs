using Identity.Business.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace Identity.Business.Concrete
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordSettings? passwordSettings;
        private readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public PasswordService(IConfiguration configuration)
        {
            passwordSettings = configuration.GetSection("PasswordSettings").Get<PasswordSettings>();
        }

        public string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[passwordSettings.SaltSize];
                rng.GetBytes(salt);

                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    passwordSettings.Iterations,
                    HashAlgorithm,
                    passwordSettings.KeySize);

                var hashBytes = new byte[passwordSettings.SaltSize + passwordSettings.KeySize];
                Array.Copy(salt, 0, hashBytes, 0, passwordSettings.SaltSize);
                Array.Copy(hash, 0, hashBytes, passwordSettings.SaltSize, passwordSettings.KeySize);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[passwordSettings.SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, passwordSettings.SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                passwordSettings.Iterations,
                HashAlgorithm,
                passwordSettings.KeySize);

            for (int i = 0; i < passwordSettings.KeySize; i++)
            {
                if (hashBytes[i + passwordSettings.SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
