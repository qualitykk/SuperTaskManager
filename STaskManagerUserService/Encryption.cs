using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerUserService
{
    internal static class Encryption
    {
        // https://codereview.stackexchange.com/questions/202917/secure-password-hashing-implementation-with-salt-and-pepper

        private const string PEPPER = "d3a39a80d06f80f077fd48a7ec0da28fe72927e0f4045f0934d77731a45f5c65";
        private const int ITERS = 1000000;
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public static bool Check(string expected, string password)
        {
            string[] hashParts = expected.Split(':');
            byte[] salt = Convert.FromBase64String(hashParts[0]);
            byte[] hash = Convert.FromBase64String(hashParts[1]);
            byte[] testHash = BuildHash(Encoding.UTF8.GetBytes(password), salt);

            uint differences = (uint)hash.Length ^ (uint)testHash.Length;
            for (int position = 0; position < Math.Min(hash.Length, testHash.Length); position++)
                differences |= (uint)(hash[position] ^ testHash[position]);
            return differences == 0;
        }

        public static byte[] BuildHash(byte[] bytesToHash, byte[] salt)
        {
            using var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(PEPPER));
            var hash = hmac.ComputeHash(bytesToHash);

            var byteResult = new Rfc2898DeriveBytes(hash, salt, ITERS);
            return byteResult.GetBytes(32);
        }

        public static string GenerateSalt()
        {
            var bytes = new byte[128 / 8];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
