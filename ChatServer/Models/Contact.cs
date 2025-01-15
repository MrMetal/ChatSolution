namespace ChatServer.Models;

public class Contact
{
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public List<ChatMessage> Messages { get; private set; } = [];
}