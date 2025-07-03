using Azure.Core;
using Data;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System;

namespace ChatServer;

//public class ChatHub : Hub
//{
//    private static readonly Dictionary<string, string> OnlineUsers = new(); // Username -> ConnectionId
//    private readonly IContactRepository _contactRepository;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly ApplicationDbContext _context;

//    public ChatHub(IContactRepository contactRepository, IUnitOfWork unitOfWork, ApplicationDbContext context)
//    {
//        _contactRepository = contactRepository;
//        _unitOfWork = unitOfWork;
//        _context = context;
//    }

//    // Called when a user connects
//    public override async Task OnConnectedAsync()
//    {
//        var email = Context.User?.Identity?.Name;
//        if (string.IsNullOrEmpty(email)) return;

//        OnlineUsers[email] = Context.ConnectionId;

//        // Atualiza status no banco
//        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
//        if (contact is not null)
//        {
//            contact.Status = "Online";
//            await _context.SaveChangesAsync();
//        }

//        await UpdateUserList();
//        await base.OnConnectedAsync();
//    }

//    public async Task Register(string user)
//    {
//        if (!OnlineUsers.Values.Contains(user))
//        {
//            OnlineUsers[Context.ConnectionId] = user;
//            await UpdateUserList();
//        }
//    }

//    public override async Task OnDisconnectedAsync(Exception? exception)
//    {
//        var email = OnlineUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

//        if (!string.IsNullOrEmpty(email))
//        {
//            OnlineUsers.Remove(email);

//            // Atualiza no banco
//            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
//            if (contact is not null)
//            {
//                contact.Status = "Offline";
//                await _context.SaveChangesAsync();
//            }
//        }

//        await UpdateUserList();
//        await base.OnDisconnectedAsync(exception);
//    }

//    public async Task SendMessage(string from, string to, string message)
//    {
//        Console.WriteLine(message);

//        if (OnlineUsers.TryGetValue(from, out var connectionId))
//        {
//            await Clients.Client(connectionId).SendAsync("MessageReceived", from, to, message);
//        }

//        if (OnlineUsers.TryGetValue(to, out var connectionsId))
//        {
//            await Clients.Client(connectionsId).SendAsync("MessageReceived", from, to, message);
//        }
//    }

//    private async Task UpdateUserList()
//    {
//        var userList = OnlineUsers.Keys.ToList();
//        await Clients.All.SendAsync("UserListUpdated", userList);
//    }

//    public async Task AddContact(ContactRequest request)
//    {
//        var entity = new Contacts
//        {
//            ConnectionId = string.Empty,
//            Name = request.Name,
//            Email = request.Email,
//            Status = "Online"
//        };

//        _contactRepository.Adicionar(entity);
//        await _unitOfWork.SaveChangesAsync();
//    }
//}


public class ChatHub : Hub
{
    // Email → ConnectionId
    private static readonly Dictionary<string, string> OnlineUsers = new();

    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public ChatHub(IContactRepository contactRepository, IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task Register(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        OnlineUsers[email] = Context.ConnectionId;

        // Atualiza status no banco
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
        if (contact != null)
        {
            contact.Status = "Online";
            await _context.SaveChangesAsync();
        }

        await BroadcastOnlineUsers();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var email = OnlineUsers.FirstOrDefault(kv => kv.Value == Context.ConnectionId).Key;

        if (!string.IsNullOrEmpty(email))
        {
            OnlineUsers.Remove(email);

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
            if (contact != null)
            {
                contact.Status = "Offline";
                await _context.SaveChangesAsync();
            }
        }

        await BroadcastOnlineUsers();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string from, string to, string message)
    {
        if (OnlineUsers.TryGetValue(to, out var toConnectionId))
        {
            await Clients.Client(toConnectionId).SendAsync("MessageReceived", from, to, message);
        }

        if (OnlineUsers.TryGetValue(from, out var fromConnectionId))
        {
            await Clients.Client(fromConnectionId).SendAsync("MessageReceived", from, to, message);
        }
    }

    public async Task AddContact(ContactRequest request)
    {
        var entity = new Contacts
        {
            ConnectionId = string.Empty,
            Name = request.Name,
            Email = request.Email,
            Status = "Offline",
            OwnerEmail = request.OwnerEmail
        };

        _contactRepository.Adicionar(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task BroadcastOnlineUsers()
    {
        var emailsOnline = OnlineUsers.Keys.ToList();
        await Clients.All.SendAsync("UserStatusUpdated", emailsOnline);
    }
}
