using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public class AuthRequest
    {
        public int UserID;
        public long Auth;
        public AuthRequest(int userID, long auth)
        {
            UserID = userID;
            Auth = auth;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{UserID};{Auth}");
        }

        public static AuthRequest FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            AuthRequest request = new(int.Parse(parts[0]), long.Parse(parts[1]));

            return request;
        }
    }
}
