using ChatService.Models;
using Persistence.Messaging.Types;

namespace ChatService;

public interface IChatClient
{
    public Task<IReadOnlyCollection<Conversation>> GetConversations();

    Task<Conversation?> GetConversationBetweenUsers(IReadOnlySet<Guid> userIds);

    public Task<MessageDTO> SendMessage(Guid senderId, IReadOnlySet<Guid> recipients, string body);
}