using Microsoft.AspNetCore.SignalR;

namespace ChatServer;

public class ChatHub : Hub
{
    private static Dictionary<string, string> Users = new(); // Username -> ConnectionId

    // Called when a user connects
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    // Called when a user registers
    public async Task Register(string username)
    {
        Users[username] = Context.ConnectionId;
        await Clients.All.SendAsync("UserListUpdated", Users.Keys.ToList());
    }

    public async Task SendMessage(string from, string to, string message)
    {
        Console.WriteLine(message);
        //await Clients.All.SendAsync("MessageReceived", from, to, message);

        //var xxx = Users.Where(x => x.Key == from).Select(c => c.Value).Single();
        //await Clients.Client(xxx).SendAsync("MessageReceived", from, to, message);

        if (Users.TryGetValue(from, out var fromConnectionId))
        {
            await Clients.Client(fromConnectionId).SendAsync("MessageReceived", from, to, message);
        }

        if (Users.TryGetValue(to, out var toConnectionId))
        {
            await Clients.Client(toConnectionId).SendAsync("MessageReceived", from, to, message);
        }
    }
}