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
        /// <summary>
        /// Turns a hashed password into a hashed & salted password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            var salt = GenerateSalt();
            var hash = BuildHash(Encoding.ASCII.GetBytes(password), salt);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Checks if a specified password matches the expected values.
        /// </summary>
        /// <param name="expected">Saved password data in salt:hash format</param>
        /// <param name="password">Hashed password to check</param>
        /// <returns></returns>
        public static bool Check(string expected, string password)
        {
            string[] hashParts = expected.Split(':');
            byte[] salt = Convert.FromBase64String(hashParts[0]); // extracted salt
            byte[] hash = Convert.FromBase64String(hashParts[1]); // expected hash

            // salt the password to check
            byte[] testHash = BuildHash(Encoding.ASCII.GetBytes(password), salt);

            uint differences = (uint)hash.Length ^ (uint)testHash.Length;
            for (int position = 0; position < Math.Min(hash.Length, testHash.Length); position++)
                differences |= (uint)(hash[position] ^ testHash[position]);
            return differences == 0;
        }
        /// <summary>
        /// Creates a usable hash with specified salt
        /// </summary>
        /// <param name="bytesToHash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] BuildHash(byte[] bytesToHash, byte[] salt)
        {
            using var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(PEPPER));
            var hash = hmac.ComputeHash(bytesToHash);

            var byteResult = new Rfc2898DeriveBytes(hash, salt, ITERS);
            return byteResult.GetBytes(32);
        }
        public static byte[] GenerateSalt()
        {
            var bytes = new byte[128 / 8];
            rng.GetBytes(bytes);
            return bytes;
        }
    }
}
