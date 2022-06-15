using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public class LoginResponse
    {
        public LoginStatus Status;
        public bool Successful => Status == LoginStatus.Accepted;
        /// <summary>
        /// The ID of the user that was logged on to, if successful
        /// </summary>
        public int? Userid;
        public long? Auth;

        public LoginResponse(LoginStatus status, int? userid = 0, long? auth = 0)
        {
            Status = status;
            Userid = userid;
            Auth = auth;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{(int)Status};{Userid}");
        }

        public static LoginResponse FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            return new LoginResponse((LoginStatus)int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static LoginResponse Failed()
        {
            return new(LoginStatus.Denied);
        }

        public static LoginResponse Success(int user, long auth)
        {
            return new(LoginStatus.Accepted, user, auth);
        }

        public static implicit operator bool(LoginResponse response)
        {
            return response.Successful;
        }
    }

    public enum LoginStatus
    {
        Unavailable,
        Denied,
        Accepted
    }
}
