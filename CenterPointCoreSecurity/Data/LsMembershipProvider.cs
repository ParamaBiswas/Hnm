using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CenterPointCoreSecurity.Data
{
    public class LsMembershipProvider
    {
        private const int _RandomMinLimit = 3;
        private const int _RandomMaxLimit = 10;
        public string GeneratePasswordSaltingText()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            int size = random.Next(_RandomMinLimit, _RandomMaxLimit);
            char ch = char.MinValue;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public string SaltText(string textToSalt, string saltedText)
        {
            return (saltedText + textToSalt + saltedText);
        }
        public string EncodeToBase64String(string text)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(text));
        }
        public string DecodeFromBase64String(string encodedText)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(encodedText));
        }
        public string EncodeText(string text)
        {
            string encodedText = string.Empty;
            byte[] clearBytes = new UnicodeEncoding().GetBytes(text);
            byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            encodedText = BitConverter.ToString(hashedBytes);


            return encodedText;
        }
    }
}
