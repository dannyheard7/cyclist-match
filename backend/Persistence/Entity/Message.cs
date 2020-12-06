using System;

namespace Persistence.Entity
{
    public class Message
    {
        public Message(IUser sender, IUser receiver, bool receiverRead, string text, DateTime sentAt)
        {
            Sender = sender;
            Receiver = receiver;
            ReceiverRead = receiverRead;
            Text = text;
            SentAt = sentAt;
        }

        public IUser Sender { get; }
        public IUser Receiver { get; }
        public bool ReceiverRead { get; }
        public string Text { get; }
        public DateTime SentAt { get; }
    }
}