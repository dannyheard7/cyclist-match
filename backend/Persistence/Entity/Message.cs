using System;

namespace Persistence.Entity
{
    public class Message
    {
        public Message(IUser sender, IUser receiver, string text, DateTime sentAt)
        {
            Sender = sender;
            Receiver = receiver;
            Text = text;
            SentAt = sentAt;
        }

        public IUser Sender { get; }
        public IUser Receiver { get; }
        public string Text { get; }
        public DateTime SentAt { get; }
    }
}