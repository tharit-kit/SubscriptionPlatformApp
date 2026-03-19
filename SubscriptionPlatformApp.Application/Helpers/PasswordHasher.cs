using SubscriptionPlatformApp.Application.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers
{
    public static class PasswordHasher
    {
        public static string GenerateHash(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                            Encoding.UTF8.GetBytes(salt),
                            PasswordConstants.ITERATION_SIZE, // Iterations
                            HashAlgorithmName.SHA512,  // Algorithm
                            PasswordConstants.KEY_SIZE); // keysize

            var hashedStr = Convert.ToBase64String(hash);

            return hashedStr;
        }

        public static string GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();

            byte[] salt = new byte[PasswordConstants.KEY_SIZE];

            rng.GetBytes(salt);

            string cryptSalt = Convert.ToBase64String(salt);

            return cryptSalt;
        }
    }
}
