namespace Domain.Models;

public class Contacts
{
    public Guid Id { get; set; }
    public string ConnectionId { get; set; } // SignalR connection ID
    public string Email { get;  set; }
    public string Name { get;  set; }
    public string Phone { get;  set; }
    public string Status { get; set; } = "Online"; // Default to online

    public List<ChatMessage> Messages { get;  set; } = [];
}