using Azure.Core;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace ChatServer.Services;

public interface IChatMessageService
{
    Task<IEnumerable<ChatMessageResult>> ObterTodos();
}

public class ChatMessageService(ApplicationDbContext context) : IChatMessageService
{
    public async Task<IEnumerable<ChatMessageResult>> ObterTodos()
    {
        var result = await context.ChatMessages.AsNoTracking().Select(x => new ChatMessageResult
        {
            From = x.From,
            To = x.To,
            Message = x.Message,
            IsMine = x.IsMine,
            ContactId = x.ContactId
        }).ToArrayAsync();

        return result;
    }
}