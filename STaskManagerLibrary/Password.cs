using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public static class Password
    {
        private static SHA256 algorithm = SHA256.Create();
        public static byte[] Encrypt(string pass)
        {
            var hash = HashString(pass);
            return hash;
        }

        public static byte[] HashString(string pass)
        {
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(pass));
        }
    }
}
