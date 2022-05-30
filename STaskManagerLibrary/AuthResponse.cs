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
        /// <summary>
        /// The string this user authenticates with
        /// </summary>
        public string? Auth;

        public AuthResponse(AuthStatus status, int? userid = 0, string? auth = null)
        {
            Status = status;
            Userid = userid;
            Auth = auth;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes($"{(int)Status};{Userid};{Auth}");
        }

        public static AuthResponse FromBytes(byte[] buffer)
        {
            var parts = Encoding.UTF8.GetString(buffer).Split(';');
            return new AuthResponse((AuthStatus)int.Parse(parts[0]), int.Parse(parts[1]), parts[2]);
        }

        public static AuthResponse Failed()
        {
            return new(AuthStatus.Denied);
        }

        public static AuthResponse Success(int user, string auth)
        {
            return new(AuthStatus.Accepted, user, auth);
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
