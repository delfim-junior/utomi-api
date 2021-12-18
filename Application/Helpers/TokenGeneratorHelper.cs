using System;
using System.Security.Cryptography;
using Application.Interfaces;

namespace Application.Helpers
{
    public class TokenGeneratorHelper : ITokenGenerator
    {
        public string GenerateSecureString(int bytes)
        {
            using var rng = RandomNumberGenerator.Create();
            
            var tokenData = new byte[bytes];
            rng.GetBytes(tokenData);

            return MakeBase64StringUrlSafe(Convert.ToBase64String(tokenData));
        }

        private static string MakeBase64StringUrlSafe(string input)
        {
            return input.TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}