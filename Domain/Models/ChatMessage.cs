﻿namespace Domain.Models;

public class ChatMessage : Entity
{
    public string From { get; init; }
    public string To { get; init; }
    public string Message { get; init; }
    public bool IsMine { get; init; }

    public Guid ContactId { get; set; }
    public Contacts Contact { get; set; }
}