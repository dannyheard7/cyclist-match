using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Persistence.Entity;
using Persistence.Repository;
using Persistence.SQL.Objects;

namespace Persistence.SQL.Repository
{
    internal class MessageRepository : IMessageRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        
        public MessageRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> GetNumberConversationsWithUnreadMessages(IUser user)
        {
            await using var connection = _connectionFactory.Create();
            return await connection.QueryFirstAsync<int>(
                @"SELECT
                        COUNT(*)
                        FROM conversation_user cu
                        WHERE cu.user_id=@UserId
                        AND latest_message_read=false",
                new
                {
                    UserId = user.Id
                }
            );
        }
        
        private class ConversationQueryResult
        {
            public Guid ConversationId { get; }
            public Guid UserId { get; }
            public string GivenNames { get; }
            public string FamilyName { get; }
            public string? Picture { get; }
            public string Email { get; }

            public Guid MessageId { get; }
            public string Text { get; }
            public Guid SenderUserId { get; }
            public DateTime SentAt { get; }
            public bool LatestMessageRead { get; }
        }

        public async Task<IEnumerable<Conversation>> GetUserConversations(IUser user)
        {
            await using var connection = _connectionFactory.Create();
            var results = await connection.QueryAsync<ConversationQueryResult>(
                @"SELECT  * FROM user_conversations(@UserId) LIMIT 10",
                new
                {
                    UserId = user.Id
                }
            );

            return results
                .GroupBy(x => x.ConversationId)
                .Select(x =>
                {
                    var users = new List<DBUser>();
                    Message? message = null;

                    foreach (var result in x)
                    {
                        users.Add(new DBUser(result.UserId, "", result.GivenNames, result.FamilyName, result.Email, result.Picture));
                        if (result.UserId == user.Id)
                        {
                            message = new Message(result.MessageId, result.SenderUserId, result.LatestMessageRead, result.Text, result.SentAt);
                        }
                    }
                    
                    return new Conversation(
                        x.Key,
                        users,
                        message != null ? new List<Message> { message } : Enumerable.Empty<Message>()
                    );
                });
        }

        public async Task<Conversation?> GetConversationBetweenUsers(IEnumerable<IUser> users, int? maxMessages=10)
        {
            await using var connection = _connectionFactory.Create();

            var conversationId = await connection.QueryFirstOrDefaultAsync<Guid?>(
                @"SELECT
                        conversation_id
                     FROM conversation_user 
                     WHERE user_id = ANY(@UserIds)
                     GROUP BY conversation_id HAVING count(*) = @UsersCount",
                new
                {
                    UserIds = users.Select(x => x.Id).ToList(),
                    UsersCount = users.Count()
                });
            if (conversationId == null) return null;
            
            var messages = await connection.QueryAsync<ConversationMessageQueryResult>(
                @"SELECT
                        cm.id as message_id,
                        cm.text,
                        cm.sent_at,
                        cm.sender_user_id
                        FROM conversation c
                        INNER JOIN conversation_message cm on c.id = cm.conversation_id
                        WHERE c.id=@ConversationId
                        ORDER BY c.id, cm.sent_at ASC
                        LIMIT @Limit
                    ",
                new
                {
                    ConversationId = conversationId,
                    Limit = maxMessages
                }
            );

            return new Conversation(
                conversationId.Value,
                users,
                messages.Select(m => new Message(m.MessageId, m.SenderUserId, true, m.Text, m.SentAt))
            );
        }

        private class ConversationMessageQueryResult
        {
            public Guid MessageId { get;  }
            public Guid SenderUserId { get; }
            public string Text { get; }
            public DateTime SentAt { get; }
        }
        
        private class ConversationUserQueryResult
        {
            public Guid UserId { get; }
            public string GivenNames { get; }
            public string FamilyName { get; }
            public string? Picture { get; }
            public string Email { get; }
            public bool LatestMessageRead { get; }
        }

        public async Task<Conversation?> GetConversationById(Guid conversationId, IUser currentUser, int? maxMessages=10)
        {
            await using var connection = _connectionFactory.Create();
            var messages = await connection.QueryAsync<ConversationMessageQueryResult>(
                @"SELECT
                        cm.id as message_id,
                        cm.text,
                        cm.sent_at,
                        cm.sender_user_id
                        FROM conversation c
                        INNER JOIN conversation_message cm on c.id = cm.conversation_id
                        WHERE cm.conversation_id=@ConversationId
                        ORDER BY cm.sent_at DESC
                        LIMIT @Limit
                    ",
                new
                {
                    ConversationId = conversationId,
                    Limit=maxMessages
                }
            );

            var users = await connection.QueryAsync<ConversationUserQueryResult>(
                @"SELECT
                        cu.user_id,
                        u.given_names,
                        u.family_name,
                        u.picture,
                        u.email,
                        cu.latest_message_read
                        FROM conversation_user cu
                        INNER JOIN ""user"" u on cu.user_id = u.id
                        WHERE cu.conversation_id=@ConversationId
                    ",
                new
                {
                    ConversationId = conversationId
                }
            );

            if (!users.Any()) return null;
            
            return new Conversation(
                conversationId,
                users.Select(u => new DBUser(u.UserId, "", u.GivenNames, u.FamilyName, u.Email, u.Picture)),
                messages.Select(m => new Message(m.MessageId, m.SenderUserId, true, m.Text, m.SentAt))
            );
        }

        public async Task MarkUnreadMessagesInConversationForUserAsRead(Conversation conversation, IUser user)
        {
            await using var connection = _connectionFactory.Create();
            await connection.ExecuteAsync(
                @"UPDATE conversation_user
                      SET latest_message_read=true
                      WHERE user_id=@UserId
                      AND conversation_id=@ConversationId
                    ", new
                {
                    UserId = user.Id,
                    ConversationId = conversation.Id
                });
        }

        public async Task CreateConversation(Conversation conversation)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await using var connection = _connectionFactory.Create();
            await connection.ExecuteAsync(
                @"INSERT INTO conversation (id) VALUES (@ConversationId)",
                new
                {
                    ConversationId=conversation.Id,
                });
            
            
            var parameters = conversation.Users.Select(u =>
            {
                var tempParams = new DynamicParameters();
                tempParams.Add("@UserId", u.Id, DbType.Guid, ParameterDirection.Input);
                tempParams.Add("@ConversationId", conversation.Id, DbType.Guid, ParameterDirection.Input);
                return tempParams;
            });

            await connection.ExecuteAsync(
                "INSERT INTO conversation_user (user_id, conversation_id) VALUES (@UserId, @ConversationId)",
                parameters
            );
            
            transaction.Complete();
        }

        public async Task SendMessage(Conversation conversation, Message message)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await using var connection = _connectionFactory.Create();
            await connection.ExecuteAsync(
                @"INSERT INTO conversation_message (conversation_id, sender_user_id, text, sent_at) VALUES (@ConversationId, @SenderUserId, @Text, @SentAt)",
                new
                {
                    ConversationId=conversation.Id,
                    SenderUserId = message.SenderUserId,
                    Text = message.Text,
                    SentAt = message.SentAt
                });
            
            await connection.ExecuteAsync(
                @"UPDATE conversation_user
                      SET latest_message_read=false
                      WHERE user_id <> @SenderUserId
                      AND conversation_id=@ConversationId
                    ", new
                {
                    SenderUserId = message.SenderUserId,
                    ConversationId = conversation.Id
                });
            
            transaction.Complete();
        }

        public async Task DeleteUserConversations(IUser user)
        {
            await using var connection = _connectionFactory.Create();
            await connection.ExecuteAsync(@"
                DELETE FROM conversation c
                USING conversation_user cu 
                WHERE c.id = cu.conversation_id
                AND cu.user_id=@UserId", 
                new {UserId=user.Id});
        }
    }
}