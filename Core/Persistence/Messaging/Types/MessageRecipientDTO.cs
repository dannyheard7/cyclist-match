using System;

namespace Persistence.Messaging.Types;

public class MessageRecipientDTO
{
    public MessageRecipientDTO(Guid recipientId, DateTime? readAt)
    {
        RecipientId = recipientId;
        ReadAt = readAt;
    }

    public Guid RecipientId { get; }
    
    public DateTime? ReadAt { get; }
}