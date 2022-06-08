using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerUserService
{
    internal static class Auth
    {
        private static Dictionary<int, long> userAuth = new();
        static Random rand = new();
        public static long Get(int uid)
        {
            long auth;
            if (userAuth.TryGetValue(uid, out auth))
            {
                return auth;
            }

            do
            {
                auth = rand.NextInt64();
            }
            while (userAuth.ContainsValue(auth));

            userAuth.Add(uid, auth);
            return auth;
        }

        public static bool Check(int uid, long auth)
        {
            return userAuth.ContainsKey(uid) && userAuth[uid] == auth;
        }
    }
}
