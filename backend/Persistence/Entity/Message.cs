using System;

namespace Persistence.Entity
{
    public class Message
    {
        public Message(Guid id, Guid senderUserId, bool receiverRead, string text, DateTime sentAt)
        {
            Id = id;
            SenderUserId = senderUserId;
            ReceiverRead = receiverRead;
            Text = text;
            SentAt = sentAt;
        }

        public Guid Id { get; }
        public Guid SenderUserId { get; }
        public bool ReceiverRead { get; }
        public string Text { get; }
        public DateTime SentAt { get; }

        public static Message Create(IUser sender, string message)
        {
            return new Message(Guid.NewGuid(), sender.Id!.Value, false, message, DateTime.Now);
        }
    }
}