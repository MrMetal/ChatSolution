using Microsoft.AspNetCore.SignalR;

namespace ChatServer;

public class ChatHub : Hub
{
    public async Task SendMessage(string from, string to, string message)
    {
        Console.WriteLine(message);
        await Clients.All.SendAsync("MessageReceived", from, to, message);
    }
}