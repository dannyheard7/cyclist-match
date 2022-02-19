using ChatService.Models;

namespace ChatService;

public interface IChatClient
{
    public Task<IReadOnlyCollection<Conversation>> GetConversations();
}