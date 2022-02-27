using System.Linq;
using Persistence.Messaging.Types;
using Persistence.SQL.Messaging.Entities;
using Persistence.SQL.Profile.Mapper;

namespace Persistence.SQL.Messaging.Mapper;

internal static class MessageMapper
{
    public static MessageDTO Map(this MessageEntity messageEntity)
    {
        var recipient = messageEntity.Recipients.First();
        return new MessageDTO(
            id: messageEntity.Id,
            conversationId: messageEntity.ConversationId,
            sender: messageEntity.SenderId,
            recipient: recipient.RecipientId,
            sentAt: messageEntity.SentAt,
            readAt: recipient.ReadAt,
            body: messageEntity.Body);
    }
}