using BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class PasswordEncodingService : IEncodingService
    {
        public byte[] CalculateSHA256(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            UTF8Encoding objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(password));
            return hashValue;
        }
    }
}
