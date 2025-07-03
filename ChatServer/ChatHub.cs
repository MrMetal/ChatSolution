using Data;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using Shared.Results;

namespace ChatServer;

public class ChatHub(IContactRepository contactRepository,
    IUnitOfWork unitOfWork,
    ApplicationDbContext context,
    IChatMessageRepository chatMessageRepository) : Hub
{
    // Email → ConnectionId
    private static readonly Dictionary<string, string> OnlineUsers = new();

    public async Task Register(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        OnlineUsers[email] = Context.ConnectionId;

        // Atualiza status no banco
        var contact = await context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
        if (contact != null)
        {
            contact.Status = "Online";
            await context.SaveChangesAsync();
        }

        await BroadcastOnlineUsers();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var email = OnlineUsers.FirstOrDefault(kv => kv.Value == Context.ConnectionId).Key;

        if (!string.IsNullOrEmpty(email))
        {
            OnlineUsers.Remove(email);

            var contact = await context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
            if (contact != null)
            {
                contact.Status = "Offline";
                await context.SaveChangesAsync();
            }
        }

        await BroadcastOnlineUsers();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string from, string to, string message, string id)
    {
        // Salva a mensagem no banco
        var chatMessage = new ChatMessage
        {
            From = from,
            To = to,
            Message = message,
            ContactId = Guid.Parse(id)
        };
        chatMessageRepository.Adicionar(chatMessage);
        await unitOfWork.SaveChangesAsync();

        var messageId = chatMessage.Id.ToString();

        // Envia a mensagem para o destinatário
        if (OnlineUsers.TryGetValue(to, out var toConnectionId))
        {
            await Clients.Client(toConnectionId).SendAsync("MessageReceived", from, to, message, messageId);
        }

        // Envia para o remetente também, para confirmação
        if (OnlineUsers.TryGetValue(from, out var fromConnectionId))
        {
            await Clients.Client(fromConnectionId).SendAsync("MessageReceived", from, to, message, messageId);
        }
    }

    // Novo método para buscar mensagens
    public async Task<List<ChatMessageResult>> GetMessageHistory(string user1, string user2)
    {
        return await context.ChatMessages
            .Where(m =>
                (m.From == user1 && m.To == user2) ||
                (m.From == user2 && m.To == user1))
            .OrderBy(m => m.DataCadastro)
            .Select(x => new ChatMessageResult
            {
                From = x.From,
                To = x.To,
                Message = x.Message
            })
            .ToListAsync();
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

        contactRepository.Adicionar(entity);
        await unitOfWork.SaveChangesAsync();

        // Envia evento para clientes atualizarem a lista de contatos
        await Clients.All.SendAsync("UserListUpdated");
    }

    private async Task BroadcastOnlineUsers()
    {
        var emailsOnline = OnlineUsers.Keys.ToList();
        await Clients.All.SendAsync("UserStatusUpdated", emailsOnline);
    }
}
