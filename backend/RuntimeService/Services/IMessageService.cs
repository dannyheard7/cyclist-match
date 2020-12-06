using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence;
using Persistence.Entity;

namespace RuntimeService.Services
{
    public interface IMessageService
    {
        public Task<int> GetNumberConversationsWithUnreadMessages(IUser user);
        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user);
    }
}