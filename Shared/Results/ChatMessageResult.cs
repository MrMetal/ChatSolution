using Domain.Models;

namespace Shared.Results;

public class ChatMessageResult : Entity
{
    public string From { get; init; }
    public string To { get; init; }
    public string Message { get; init; }
    public bool IsMine { get; init; }

    public Guid ContactId { get; set; }
}