using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public struct AuthResponse
    {
        public AuthStatus Status;
        public bool Successful => Status == AuthStatus.Accepted;
        /// <summary>
        /// The ID of the user that was logged on to, if successful
        /// </summary>
        public int? Userid;

        public AuthResponse(AuthStatus status, int? userid = 0)
        {
            Status = status;
            Userid = userid;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{(int)Status};{Userid}");
        }

        public static AuthResponse FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            return new AuthResponse((AuthStatus)int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static AuthResponse Failed()
        {
            return new(AuthStatus.Denied);
        }

        public static AuthResponse Success(int user)
        {
            return new(AuthStatus.Accepted, user);
        }

        public static implicit operator bool(AuthResponse response)
        {
            return response.Successful;
        }
    }

    public enum AuthStatus
    {
        Unavailable,
        Denied,
        Accepted
    }
}
