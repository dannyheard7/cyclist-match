namespace ChatService.Models;

public class Conversation
{
    public Conversation(IReadOnlyCollection<string> participants, DateTime lastMessageTime)
    {
        Participants = participants;
        LastMessageTime = lastMessageTime;
    }

    public IReadOnlyCollection<string> Participants { get; }
    
    public DateTime LastMessageTime { get; }
}