using System.Security.Cryptography;

namespace com_in.server.Helper
{
    public class TokenGenerator
    {
        public static string GenerateEmailConfimationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public static bool ValidateTokenExpiry(DateTime? tokenExpiry)
        {
            return tokenExpiry.HasValue && tokenExpiry > DateTime.UtcNow;
        }
    }
}
