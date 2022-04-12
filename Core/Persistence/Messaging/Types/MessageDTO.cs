using System;
using System.Collections.Generic;
using Persistence.Profile.Types.DTO;

namespace Persistence.Messaging.Types;

public class MessageDTO
{
    public MessageDTO(
        Guid id,
        Guid conversationId,
        Guid sender,
        IReadOnlyCollection<MessageRecipientDTO> recipients,
        DateTime sentAt,
        string body)
    {
        Id = id;
        ConversationId = conversationId;
        SenderId = sender;
        Recipients = recipients;
        SentAt = sentAt;
        Body = body;
    }
    
    public Guid Id { get; }
    
    public Guid ConversationId { get; }

    public Guid SenderId { get; }
    
    public IReadOnlyCollection<MessageRecipientDTO> Recipients { get; }
    
    public DateTime SentAt { get; }

    public string Body { get; }
}