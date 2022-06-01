using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public struct AuthRequest
    {
        public string Username;
        public string Password;
        public AuthRequest(string user, string pass)
        {
            Username = user;
            Password = pass;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{Username};{Password}");
        }

        public static AuthRequest FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            AuthRequest request = new();
            request.Username = parts[1];
            request.Password = parts[2];

            return request;
        }
    }
}
