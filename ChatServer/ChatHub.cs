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
    //public async Task Register(string username)
    //{
    //    Users[username] = Context.ConnectionId;
    //    await Clients.All.SendAsync("UserListUpdated", Users.Keys.ToList());
    //}

    public async Task Register(string username)
    {
        if (!Users.Values.Contains(username))
        {
            Users[username] = Context.ConnectionId;

            await UpdateUserList();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Users.Remove(Context.ConnectionId))
        {
            await UpdateUserList();
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string from, string to, string message)
    {
        Console.WriteLine(message);

        if (Users.TryGetValue(from, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("MessageReceived", from, to, message);
        }

        if (Users.TryGetValue(to, out var connectionsId))
        {
            await Clients.Client(connectionsId).SendAsync("MessageReceived", from, to, message);
        }
    }

    private async Task UpdateUserList()
    {
        var userList = Users.Keys.ToList();
        await Clients.All.SendAsync("UserListUpdated", userList);
    }
}