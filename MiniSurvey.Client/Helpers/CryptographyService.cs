using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class CryptographyService : ICryptographyService
    {
        internal static readonly char[] Chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        #region Hashing Services
        private readonly int _hashSize = 32;
        private readonly int _hashIterations = 128;
        private byte[] CreateSalt()
        {
            byte[] salt;
            using (RNGCryptoServiceProvider rNgCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rNgCryptoServiceProvider.GetBytes(salt = new byte[_hashSize]);
            }

            return salt;
        }

        private byte[] CreateHash(string input, byte[] salt)
        {
            byte[] hash;
            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(input, salt, _hashIterations))
            {
                hash = hashGenerator.GetBytes(_hashSize);
            }

            return hash;
        }

        public HashDetail GenerateHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            byte[] salt = CreateSalt();
            byte[] hash = CreateHash(input, salt);

            return new HashDetail { Salt = Convert.ToBase64String(salt), HashedValue = Convert.ToBase64String(hash) };
        }

        public bool ValidateHash(string input, string salt, string hashedValue)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(hashedValue))
                return false;
            byte[] saltByte = Convert.FromBase64String(salt);
            byte[] inputHash = CreateHash(input, saltByte);
            string hashedString = Convert.ToBase64String(inputHash);

            if (hashedString.Equals(hashedValue))
                return true;

            return false;

        }

        #endregion

        #region Encoding and Decoding
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        #endregion       

        #region Client Credentials Services

        public string ComputeHmac256(string key, string model)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] modelBytes = encoding.GetBytes(model);

            var hmacsha256 = new HMACSHA256(key: keyBytes);
            byte[] hmacbBytesMessage = hmacsha256.ComputeHash(modelBytes);
            using (hmacsha256)
            {
                return ByteToHex(hmacbBytesMessage);
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                // String.Concat(Array.ConvertAll(hmacbBytesMessage, x => x.ToString("x2")));
                // return Convert.ToBase64String((hmacbBytesMessage));
            }
        }

        public string ByteToHex(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }
        #endregion
    }
}
