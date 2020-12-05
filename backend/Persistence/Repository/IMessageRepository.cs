using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entity;

namespace Persistence.Repository
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user);
        public Task<IEnumerable<Message>> GetConversationMessages(Conversation conversation);
    }
}