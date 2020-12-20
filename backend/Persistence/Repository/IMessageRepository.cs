using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entity;

namespace Persistence.Repository
{
    public interface IMessageRepository
    {
        public Task<int> GetNumberConversationsWithUnreadMessages(IUser user);
        public Task<IEnumerable<Conversation>> GetUserConversations(IUser user);
        public Task<Conversation?> GetConversationBetweenUsers(IEnumerable<IUser> users, int? maxMessages);
        
        public Task<Conversation?> GetConversationById(Guid conversationId, IUser currentUser, int? maxMessages);
        public Task MarkUnreadMessagesInConversationForUserAsRead(Conversation conversation, IUser user);
        
        public Task CreateConversation(Conversation conversation);
        public Task SendMessage(Conversation conversation, Message message);
    }
}