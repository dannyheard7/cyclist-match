using Persistence.Profile.Types.DTO;

namespace ChatService.Models;

public class ConversationMessage : IComparable<ConversationMessage>
{
    public ConversationMessage(Guid id, ProfileDTO sender, DateTime sentAt, DateTime? readAt, string body)
    {
        Id = id;
        Sender = sender;
        SentAt = sentAt;
        ReadAt = readAt;
        Body = body;
    }
    
    public Guid Id { get; }

    public ProfileDTO Sender { get; }
    
    public DateTime SentAt { get; }
    
    public DateTime? ReadAt { get; }

    public bool Read => ReadAt != null;
    
    public string Body { get; }

    public int CompareTo(ConversationMessage? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Id.CompareTo(other.Id);
    }
}