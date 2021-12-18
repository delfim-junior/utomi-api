using System.Security.Cryptography;

namespace Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateSecureString(int bytes);
    }
}