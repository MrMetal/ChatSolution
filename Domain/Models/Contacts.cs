﻿namespace Domain.Models;

public class Contacts : Entity
{
    public string ConnectionId { get; set; } // SignalR connection ID
    public string Email { get;  set; }
    public string Name { get;  set; }
    public string Status { get; set; } = "Online"; // Default to online
    public string OwnerEmail { get; set; } = ""; // <--- novo campo


    public List<ChatMessage> Messages { get;  set; } = [];
}