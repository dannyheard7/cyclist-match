using Persistence.Profile.Types.DTO;

namespace ChatService.Models;

public class ConversationMessage : IComparable<ConversationMessage>
{
    public ConversationMessage(ProfileDTO sender, DateTime sentAt, DateTime? readAt, string body)
    {
        Sender = sender;
        SentAt = sentAt;
        ReadAt = readAt;
        Body = body;
    }

    public ProfileDTO Sender { get; }
    
    public DateTime SentAt { get; }
    
    public DateTime? ReadAt { get; }

    public bool Read => ReadAt != null;
    
    public string Body { get; }
    
    public int CompareTo(ConversationMessage? other)
    {
        throw new NotImplementedException();
    }
}