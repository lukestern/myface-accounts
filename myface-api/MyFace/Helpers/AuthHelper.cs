using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MyFace.Helpers
{
    public static class AuthHelper
    {
        public static string DecodeFrom64(string encodedData)
        {
            var encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static byte[] GetSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public static string[] GetUsernamePasswordFromAuthHeader(string authHeader)
        {
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            return encoding.GetString(Convert.FromBase64String(encodedUsernamePassword)).Split(":");
        }
    }
}