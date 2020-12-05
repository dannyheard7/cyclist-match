using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence;
using Persistence.Entity;
using Persistence.Repository;

namespace RuntimeService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user)
        {
            return _messageRepository.GetUserConversations(user);
        }
    }
}