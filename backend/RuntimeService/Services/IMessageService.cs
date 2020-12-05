using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence;
using Persistence.Entity;

namespace RuntimeService.Services
{
    public interface IMessageService
    {
        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user);
    }
}