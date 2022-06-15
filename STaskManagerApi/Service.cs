using STaskManagerLibrary;
using System.Net;
using System.Net.Sockets;
using static STaskManagerUserService.Program;

namespace STaskManagerApi
{
    internal static class Service
    {
        internal static class Auth
        {
            private static readonly UdpClient _client = new();
            private static readonly IPEndPoint remoteEndpoint = new(IPAddress.Loopback, PORT);
            public static async Task<LoginResponse> TryLogonAsync(string user, string pass)
            {
                LoginRequest request = new(user, pass);
                var buffer = new List<byte>() { 0x1 };
                buffer.AddRange(request.ToBytes());

                await _client.SendAsync(buffer.ToArray(), buffer.Count, remoteEndpoint );
                
                var response = await _client.ReceiveAsync();
                return LoginResponse.FromBytes(response.Buffer);
            }

            public static async Task<bool> TryAuthAsync(int uid, string auth)
            {
                throw new NotImplementedException();
            }
        }
    }
}
