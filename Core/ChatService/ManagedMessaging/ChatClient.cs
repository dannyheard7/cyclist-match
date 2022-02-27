using System.Transactions;
using ChatService.Models;
using Common;
using Persistence.Filter;
using Persistence.Messaging;
using Persistence.Messaging.Types;
using Persistence.Profile.Filter;
using Persistence.Profile.Types.DTO;
using ProfileService;

namespace ChatService.ManagedMessaging;

internal class ChatClient : IChatClient
{
    private readonly IMessagingRepository _messagingRepository;
    private readonly IProfileService _profileService;
    
    public ChatClient(IMessagingRepository messagingRepository, IProfileService profileService)
    {
        _messagingRepository = messagingRepository;
        _profileService = profileService;
    }

    public Task<IReadOnlyCollection<Conversation>> GetConversations()
    {
        throw new NotImplementedException();
    }

    public async Task<Conversation?> GetConversationBetweenUsers(IReadOnlySet<Guid> userIds)
    {
        if (userIds.Count < 2)
        {
            throw new ArgumentException("Not enough userIds specified", nameof(userIds));
        }
        
        var conversationId = await _messagingRepository.GetConversationId(userIds.ToList());
        if (conversationId == null)
        {
            return null;
        }
        
        var allMessages = await _messagingRepository.GetConversationMessages(conversationId.Value);

        var participantIds =
            allMessages
                .SelectMany(message => new List<Guid> { message.RecipientId, message.SenderId })
                .ToHashSet();
        var participantResults = await _profileService.Get(new ProfileFilter(idFilter: GuidFilter.WithAnyOf(participantIds)));
        var participantsById = participantResults.ToDictionary(x => x.UserId, x => x);

        HashSet<ConversationMessage> messages = new HashSet<ConversationMessage>();
        foreach (var message in allMessages)
        {
            var sender = participantsById[message.SenderId];
            messages.Add(new ConversationMessage(sender, message.SentAt, message.ReadAt, message.Body));
        }

        return new Conversation(participantsById.Values.ToHashSet(), messages);
    }

    public async Task<MessageDTO> SendMessage(Guid senderId, IReadOnlySet<Guid> recipients, string body)
    {
        if (recipients.Count == 0)
        {
            throw new ArgumentException("Must specify at least one recipient", nameof(recipients));
        }
        
        if (recipients.Contains(senderId))
        {
            throw new ArgumentException("The sender cannot be a recipient", nameof(recipients));
        }
        
        using var transactionsScope = TransactionScopeBuilder.CreateReadCommitted(TransactionScopeAsyncFlowOption.Enabled);

        var allIds = new List<Guid>(recipients) { senderId };
        
        var conversationId = await _messagingRepository.GetConversationId(allIds);
        if (conversationId == null)
        {
            conversationId = await _messagingRepository.CreateConversation();
        }

        var sentAt = DateTime.UtcNow;
        var messageId = Guid.NewGuid();

        List<MessageDTO> messagesToSend = new List<MessageDTO>
        {
            new MessageDTO(
                messageId,
                conversationId.Value,
                senderId,
                senderId,
                sentAt,
                sentAt,
                body
            )
        };
        
        foreach (var recipient in recipients)
        {
            messagesToSend.Add(new MessageDTO(
                messageId,
                conversationId.Value,
                senderId,
                recipient,
                sentAt,
                null,
                body
            ));
        }
        
        await _messagingRepository.CreateMessages(messagesToSend);

        transactionsScope.Complete();
        return messagesToSend.First();
    }
}