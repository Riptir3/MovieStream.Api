using System.Security.Cryptography;
using System.Text;

namespace MovieStream.Api.Services
{
    internal sealed class PasswordService
    {
        public void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                hash = Convert.ToBase64String(passwordHash);
                salt = Convert.ToBase64String(passwordSalt);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == hashedPassword;
            }
        }
    }
}
