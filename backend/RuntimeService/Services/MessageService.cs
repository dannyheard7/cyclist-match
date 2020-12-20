using System;
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

        public Task<int> GetNumberConversationsWithUnreadMessages(IUser user)
        {
            return _messageRepository.GetNumberConversationsWithUnreadMessages(user);
        }

        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user)
        {
            return _messageRepository.GetUserConversations(user);
        }

        public Task<Conversation?> GetConversationBetweenUsers(IEnumerable<IUser> users, int? maxMessages)
        {
            return _messageRepository.GetConversationBetweenUsers(users, maxMessages);
        }

        public Task<Conversation?> GetConversationById(Guid conversationId, IUser currentUser, int? maxMessages)
        {
            return _messageRepository.GetConversationById(conversationId, currentUser, maxMessages);
        }

        public async Task MarkUnreadMessagesAsRead(Conversation conversation, IUser currentUser)
        {
            await _messageRepository.MarkUnreadMessagesInConversationForUserAsRead(conversation, currentUser);
        }

        public async Task CreateConversation(Conversation conversation)
        {
            await _messageRepository.CreateConversation(conversation);
        }

        public Task SendMessage(Conversation conversation, Message message)
        {
            // TODO: send an email notification
            return _messageRepository.SendMessage(conversation, message);
        }
    }
}