using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private class MessageQueryResult
        {
            public Guid SenderUserId { get; }
            public string SenderGivenNames { get; }
            public string SenderFamilyName { get; }
            public string? SenderPicture { get; }
            public string SenderEmail { get; }
            
            public Guid ReceiverUserId { get; }
            public string ReceiverGivenNames { get; }
            public string ReceiverFamilyName { get; }
            public string? ReceiverPicture { get; }
            public string ReceiverEmail { get; }
            
            public string Text { get; }
            public DateTime SentAt { get; }

            public Message ToMessage()
            {
                var sender = new DBUser(SenderUserId, "", SenderGivenNames, SenderFamilyName, SenderPicture, SenderEmail);
                var receiver = new DBUser(ReceiverUserId, "", ReceiverGivenNames, ReceiverFamilyName, ReceiverPicture, ReceiverEmail);
                return new Message(sender, receiver, Text, SentAt);
            }
        }
        
        public async Task<IEnumerable<Conversation>> GetUserConversations(IUser user)
        {
            await using var connection = _connectionFactory.Create();
            var results = await connection.QueryAsync<MessageQueryResult>(
                @"SELECT
                        text,
                        sent_at,
                        sender_user_id,
                        sender.given_names as sender_given_names,
                        sender.family_name as sender_family_name,
                        sender.picture as sender_picture,
                        sender.email as sender_email,
                        receiver_user_id,
                        receiver.given_names as receiver_given_names,
                        receiver.family_name as receiver_family_name,
                        receiver.picture as receiver_picture,
                        receiver.email as receiver_email
                        FROM ""message"" msg
                        INNER JOIN ""user"" sender on sender.id = msg.sender_user_id
                        INNER JOIN ""user"" receiver on receiver.id = msg.receiver_user_id
                        WHERE sender_user_id=@UserId
                        OR receiver_user_id=@UserId
                        ORDER BY sent_at DESC 
                        LIMIT 1",
                new
                {
                    UserId = user.Id
                }
            );

            return results.Select(result => new Conversation(result.ToMessage()));
        }

        public Task<IEnumerable<Message>> GetConversationMessages(Conversation conversation)
        {
            throw new System.NotImplementedException();
        }
    }
}