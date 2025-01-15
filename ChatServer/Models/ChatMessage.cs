namespace ChatServer.Models;

public class ChatMessage
{
    public string From { get; init; }
    public string To { get; init; }
    public string Message { get; init; }
    public bool IsMine { get; init; }
}