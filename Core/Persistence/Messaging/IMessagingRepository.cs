using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Messaging.Types;

namespace Persistence.Messaging;

public interface IMessagingRepository
{
    Task<Guid?> GetConversationId(List<Guid> userIds);
    
    public Task<IReadOnlyCollection<MessageDTO>> GetConversationMessages(Guid conversationId);

    public Task<Guid> CreateConversation();

    public Task CreateMessages(IReadOnlyCollection<MessageDTO> messages);
}