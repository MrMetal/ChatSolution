using ChatServer.Services;
using Data;
using Domain.Models;
using Shared.Requests;
using Shared.Results;

namespace ChatServer.Endpoints;

public static class ChatMessageEndpoints
{
    public static void MapChatMessageEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("ChatMessage");
        group.MapGet("/GetAllChatMessageEndpoint", GetAllChatMessageEndpoint.ExecuteAsync).WithTags("Get All");
        group.MapPost("/PostChatMessageEndpoint", PostChatMessageEndpoint.ExecuteAsync).WithTags("Insert");
    }
}

public static class GetAllChatMessageEndpoint
{
    internal static async Task<IEnumerable<ChatMessageResult>> ExecuteAsync(IChatMessageService charMessageService)
    {
        var result = await charMessageService.ObterTodos();
        return result;
    }
}

public static class PostChatMessageEndpoint
{
    internal static async Task ExecuteAsync(ChatMessageRequest request, IChatMessageRepository chatMessageRepository, IUnitOfWork unitOfWork)
    {
        var entity = new ChatMessage
        {
            From = request.From,
            To = request.To,
            Message = request.Message,
            IsMine = request.IsMine,
            ContactId = request.ContactId
        };

        chatMessageRepository.Adicionar(entity);

        await unitOfWork.SaveChangesAsync();
    }
}