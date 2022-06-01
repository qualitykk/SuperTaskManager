﻿using System;
using System.Net;
using System.Net.Sockets;
using STaskManagerLibrary;
namespace STaskManagerUserService;
public class Program
{
    public const int PORT = 66024;
    private static TaskManagerContext _context = new();
    private static readonly UdpClient _client = new(PORT);
    public static async Task Main(string[] args)
    {
        while (true)
        {
            var r = await _client.ReceiveAsync();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            HandleRequest(r);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    public static async Task HandleRequest(UdpReceiveResult request)
    {
        var authReq = AuthRequest.FromBytes(request.Buffer);
        switch(authReq.Type)
        {
            case RequestType.Login:
                await RespondWithAsync(request.RemoteEndPoint, TryLogon(authReq.Username, authReq.Password));
                break;
            default:
                await RespondWithAsync(request.RemoteEndPoint, AuthResponse.Failed());
                break;
        }
    }

    private static async Task RespondWithAsync(IPEndPoint ip, AuthResponse response)
    {
        var buffer = response.ToBytes();
        await _client.SendAsync(buffer, buffer.Length, ip);
    }

    public static AuthResponse TryLogon(string user, string password)
    {
        var entry = _context.Account.Single(a => a.Name == user);
        if(Encryption.Check(entry.Password, password))
        {
            // TODO: Return auth to block other users from viewing our data 
            return AuthResponse.Success(entry.Uid, "");
        }
        return AuthResponse.Failed();
    }
}