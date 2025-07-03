using ChatServer.Services;
using Shared.Results;

namespace ChatServer.Endpoints;

public static class ChatMessageEndpoints
{
    public static void MapChatMessageEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("ChatMessage");
        group.MapGet("/GetAllChatMessageEndpoint", GetAllChatMessageEndpoint.ExecuteAsync).WithTags("Get All");
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