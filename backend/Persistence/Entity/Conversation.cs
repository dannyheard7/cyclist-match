namespace Persistence.Entity
{
    public class Conversation
    {
        public Conversation(Message lastMessage)
        {
            LastMessage = lastMessage;
        }

        public Message LastMessage { get; }
    }
}