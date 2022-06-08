using System;
using System.Net;
using System.Net.Sockets;
using STaskManagerLibrary;
namespace STaskManagerUserService;
#nullable disable
public class Program
{
    public static bool Active { get; private set; } = false;
    public const int PORT = 6024;
    private static TaskManagerContext _context = new();
    private static UdpClient _server;
    public static void Main(string[] args)
    {
        _server = new(PORT);

        Console.WriteLine($"STM User Service Started on port {PORT}...");
        Active = true;
        while (true)
        {
            IPEndPoint client = null;
            var buffer = _server.Receive(ref client);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Console.WriteLine($"Handling request from {client}");
            HandleRequest(client, buffer);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    public static async Task HandleRequest(IPEndPoint client, byte[] buffer)
    {
        var authReq = LoginRequest.FromBytes(buffer);
        await RespondWithAsync(client, TryLogon(authReq.Username, authReq.Password));
    }

    private static async Task RespondWithAsync(IPEndPoint ip, LoginResponse response)
    {
        var buffer = response.ToBytes();
        await _server.SendAsync(buffer, buffer.Length, ip);
        Console.WriteLine($"Responded with {response.Status} to {ip}");
    }

    public static LoginResponse TryLogon(string user, string password)
    {
        var entry = _context.Account.Single(a => a.Name == user);
        if(Encryption.Check(entry.Password, password))
        {
            int id = entry.Uid;
            long auth = Auth.Get(id);
            return LoginResponse.Success(id, auth);
        }
        return LoginResponse.Failed();
    }
}