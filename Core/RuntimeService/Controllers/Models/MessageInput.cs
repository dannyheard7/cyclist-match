using System;

namespace RuntimeService.Controllers.Models;

public class MessageInput
{
    public MessageInput(Guid recipientId, string body)
    {
        RecipientId = recipientId;
        Body = body;
    }

    public Guid RecipientId { get; }
    
    public string Body { get; }
}