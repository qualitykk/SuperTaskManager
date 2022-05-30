using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public struct AuthRequest
    {
        public RequestType Type = RequestType.None;

        #region Login
        public string Username = "";
        public string Password = "";
        #endregion

        #region Verify
        public int Userid = -1;
        public string Auth = "";
        #endregion
        public AuthRequest(string user, string pass)
        {
            Type = RequestType.Login;
            Username = user;
            Password = pass;
        }

        public AuthRequest(int userid, string auth)
        {
            Type = RequestType.Verify;
            Userid = userid;
            Auth = auth;
        }

        public byte[] ToBytes()
        {
            if(Type == RequestType.None)
            {
                throw new InvalidOperationException("Cant convert a request without type to bytes!");
            }

            List<byte> data = new();
            
            switch(Type)
            {
                case RequestType.Login:
                    data.AddRange(Encoding.UTF8.GetBytes($"{Type};{Username};{Password}"));
                    break;
                case RequestType.Verify:
                    data.AddRange(Encoding.UTF8.GetBytes($"{Type};{Userid};{Auth}"));
                    break;
            }

            return data.ToArray();
        }

        public static AuthRequest FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            var request = new AuthRequest
            {
                Type = (RequestType)int.Parse(parts[0])
            };
            switch (request.Type)
            {
                case RequestType.Login:
                    request.Username = parts[1];
                    request.Password = parts[2];
                    break;
                case RequestType.Verify:
                    request.Userid = int.Parse(parts[1]);
                    request.Auth = parts[2];
                    break;
                default:
                    throw new ArgumentException("Type given by the buffer is not valid!");
            }

            return request;
        }
    }
    public enum RequestType
    {
        None,
        Login,
        Verify
    }
}
