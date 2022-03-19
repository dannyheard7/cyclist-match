using Persistence.Profile.Types.DTO;

namespace ChatService.Models;

public class Conversation
{
    public Conversation(IReadOnlySet<ProfileDTO> participants, IReadOnlySet<ConversationMessage> messages)
    {
        Id = id;
        Participants = participants;
        Messages = messages;
    }

    public IReadOnlySet<ProfileDTO> Participants { get; }
    
    public IReadOnlySet<ConversationMessage> Messages { get; }
}