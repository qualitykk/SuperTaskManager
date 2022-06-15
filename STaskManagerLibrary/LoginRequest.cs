using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public class LoginRequest
    {
        public string Username;
        public string Password;
        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{Username};{Password}");
        }

        public static LoginRequest FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            LoginRequest request = new(parts[0], parts[1]);

            return request;
        }
    }
}
