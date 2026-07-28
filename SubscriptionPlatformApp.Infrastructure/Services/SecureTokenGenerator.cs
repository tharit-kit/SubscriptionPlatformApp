using Microsoft.AspNetCore.WebUtilities;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Services
{
    public sealed class SecureTokenGenerator : ISecureTokenGenerator
    {
        // 32 bytes = 256 bits of randomness.
        private const int TokenSizeInBytes = 32;

        public GeneratedToken Generate()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(TokenSizeInBytes);

            // Creates URL-safe Base64 without characters such as +, /, and =.
            var rawToken = WebEncoders.Base64UrlEncode(randomBytes);

            var tokenHash = Hash(rawToken);

            return new GeneratedToken(
                Value: rawToken,
                Hash: tokenHash);
        }

        public string Hash(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = SHA256.HashData(tokenBytes);

            // Produces a 64-character hexadecimal string.
            return Convert.ToHexString(hashBytes);
        }

        public bool Verify(string token, string expectedHash)
        {
            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(expectedHash))
            {
                return false;
            }

            byte[] expectedHashBytes;

            try
            {
                expectedHashBytes = Convert.FromHexString(expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }

            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var actualHashBytes = SHA256.HashData(tokenBytes);

            return CryptographicOperations.FixedTimeEquals(
                actualHashBytes,
                expectedHashBytes);
        }
    }
}
