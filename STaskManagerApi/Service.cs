using STaskManagerLibrary;
using System.Net;
using System.Net.Sockets;

namespace STaskManagerApi
{
    internal static class Service
    {
        internal static class Auth
        {
            private static readonly UdpClient _client = new();
            public static bool IsActive()
            {
                throw new NotImplementedException();
            }
            public static async Task<AuthResponse> TryLogon(string user, string pass)
            {
                _client.Connect(IPAddress.Loopback, STaskManagerUserService.Program.PORT);
                AuthRequest request = new(user, pass);
                await _client.SendAsync(request.ToBytes());

                var response = AuthResponse.FromBytes((await _client.ReceiveAsync()).Buffer);
                return response;
            }

            public static async Task<AuthResponse> Verify(string auth, int userid)
            {
                throw new NotImplementedException();
            }
        }
    }
}
