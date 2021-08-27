using Psps.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Psps.Core.Common
{
    public static class Encryptor
    {
        public static string Hash(string data)
        {
            Ensure.Argument.NotNullOrEmpty(data, "data");

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = SHA1.Create().ComputeHash(bytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}