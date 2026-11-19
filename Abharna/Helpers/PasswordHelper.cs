using System.Security.Cryptography;
using System.Text;

namespace Abharna.Helpers
{
    // turns a password into a hash, so the real password is never stored in the database
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
