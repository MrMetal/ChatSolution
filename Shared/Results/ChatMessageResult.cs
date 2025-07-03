using Domain.Models;

namespace Shared.Results;

public class ChatMessageResult : Entity
{
    public string From { get; set; }
    public string To { get; set; }
    public string Message { get; set; }
    public bool IsMine { get; set; }

    public Guid ContactId { get; set; }
}