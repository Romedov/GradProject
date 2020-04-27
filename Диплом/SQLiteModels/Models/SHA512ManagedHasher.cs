using System;
using System.Security.Cryptography;
using System.Text;

namespace Kassa.Models
{
    public static class SHA512ManagedHasher
    {
        public static string GetHash(string str, Encoding encode)
        {
            byte[] data = encode.GetBytes(str);
            var result = new SHA512Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
        public static bool HashEquals(string inputStr, string encryptedStr, Encoding encode)
        {
            byte[] data = encode.GetBytes(inputStr);
            var result = new SHA512Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower() == encryptedStr;
        }
    }
}
