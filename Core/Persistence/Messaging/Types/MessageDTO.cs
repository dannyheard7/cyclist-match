using System;
using Persistence.Profile.Types.DTO;

namespace Persistence.Messaging.Types;

public class MessageDTO
{
    public MessageDTO(
        Guid id,
        Guid conversationId,
        Guid sender,
        Guid recipient,
        DateTime sentAt,
        DateTime? readAt,
        string body)
    {
        Id = id;
        ConversationId = conversationId;
        SenderId = sender;
        RecipientId = recipient;
        SentAt = sentAt;
        ReadAt = readAt;
        Body = body;
    }
    
    public Guid Id { get; }
    
    public Guid ConversationId { get; }

    public Guid SenderId { get; }
    
    public Guid RecipientId { get; }
    
    public DateTime SentAt { get; }

    public DateTime? ReadAt { get; }

    public string Body { get; }
}