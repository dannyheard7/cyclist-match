using System;

namespace Persistence.Entity
{
    public class Message
    {
        public Message(Guid senderUserId, bool receiverRead, string text, DateTime sentAt)
        {
            SenderUserId = senderUserId;
            ReceiverRead = receiverRead;
            Text = text;
            SentAt = sentAt;
        }

        public Guid SenderUserId { get; }
        public bool ReceiverRead { get; }
        public string Text { get; }
        public DateTime SentAt { get; }

        public static Message Create(IUser sender, string message)
        {
            return new Message(sender.Id!.Value, false, message, DateTime.Now);
        }
    }
}