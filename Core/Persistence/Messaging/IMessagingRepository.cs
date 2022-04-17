using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Persistence.Messaging.Types;

namespace Persistence.Messaging;

public interface IMessagingRepository
{
    Task<Page<MessageDTO>> GetUserConversations(Guid userId, PageRequest pageRequest, bool unreadOnly);
    
    Task<Guid?> GetConversationId(IReadOnlyCollection<Guid> userIds);
    
    public Task<IReadOnlyCollection<MessageDTO>> GetConversationMessages(Guid conversationId, PageRequest pageRequest);

    public Task<Guid> CreateConversation();

    public Task CreateMessage(MessageDTO message);

    public Task UpdateMessage(MessageDTO message);
}