using System;
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
        public Task<Conversation?> GetConversationBetweenUsers(IEnumerable<IUser> users, int? maxMessages);
        
        public Task<Conversation?> GetConversationById(Guid conversationId, IUser currentUser, int? maxMessages);
        public Task MarkUnreadMessagesAsRead(Conversation conversation, IUser currentUser);
        public Task CreateConversation(Conversation conversation);
        public Task SendMessage(Conversation conversation, Message message);
        
        public Task DeleteUserConversations(IUser user);
    }
}