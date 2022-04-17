using System.Linq;
using Persistence.Messaging.Types;
using Persistence.SQL.Messaging.Entities;
using Persistence.SQL.Profile.Mapper;

namespace Persistence.SQL.Messaging.Mapper;

internal static class MessageMapper
{
    public static MessageDTO Map(this MessageEntity messageEntity)
    {
        return new MessageDTO(
            id: messageEntity.Id,
            conversationId: messageEntity.ConversationId,
            sender: messageEntity.SenderId,
            recipients: messageEntity.Recipients
                .Select(x => new MessageRecipientDTO(x.RecipientId, x.ReadAt))
                .ToList(),
            sentAt: messageEntity.SentAt,
            body: messageEntity.Body);
    }
}