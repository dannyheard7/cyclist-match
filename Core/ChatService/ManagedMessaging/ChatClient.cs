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

    private ConversationMessage Convert(
        Guid currentUserId,
        MessageDTO message,
        ProfileDTO sender)
    {
        return new ConversationMessage(
            message.Id,
            sender,
            message.SentAt,
            message.Recipients
                .Single(x => x.RecipientId == currentUserId).ReadAt,
            message.Body);
    }
    
    private async Task<IReadOnlyDictionary<Guid, ProfileDTO>> GetParticipantProfiles(IReadOnlyCollection<MessageDTO> messages)
    {
        if (!messages.Any())
        {
            return new Dictionary<Guid, ProfileDTO>();
        }

        var participantIds =
            messages
                .SelectMany(x => x.Recipients)
                .Select(x => x.RecipientId)
                .ToHashSet();
        var participantResults = await _profileService.Get(new ProfileFilter(idFilter: GuidFilter.WithAnyOf(participantIds)));
        return participantResults.ToDictionary(x => x.UserId, x => x);
    }

    private async Task MarkMessagesAsRead(IReadOnlyCollection<MessageDTO> messages, Guid userId, DateTime readAtTime)
    {
        if (readAtTime.Kind == DateTimeKind.Unspecified)
        {
            throw new ArgumentException("DateTime kind must be specified", nameof(readAtTime));
        }

        using var transactionsScope = TransactionScopeBuilder.CreateReadCommitted(TransactionScopeAsyncFlowOption.Enabled);

        foreach (var message in messages)
        {
            var currentUserRecipient = message.Recipients.Single(x => x.RecipientId == userId);
            if (!currentUserRecipient.Read)
            {
                currentUserRecipient.ReadAt = readAtTime;
            }
            
            await _messagingRepository.UpdateMessage(message);
        }

        transactionsScope.Complete();
    }

    public async Task<Page<Conversation>> GetUserConversations(Guid userId, PageRequest? pageRequest, bool unreadOnly)
    {
        var allConversationMessages = await _messagingRepository.GetUserConversations(userId, pageRequest ?? PageRequest.All, unreadOnly);
        
        var participantsById = await GetParticipantProfiles(allConversationMessages.Items);
        var conversations = allConversationMessages.Items
            .GroupBy(x => x.ConversationId)
            .Select(conversation =>
            {
                var participants = conversation
                    .SelectMany(x => x.Recipients)
                    .Select(x => x.RecipientId)
                    .Distinct()
                    .Select(participantId => participantsById[participantId])
                    .ToHashSet();

                var messages = conversation
                    .Select(message => Convert(userId, message, participantsById[message.SenderId]))
                    .ToHashSet();

                return new Conversation(participants, messages);
            })
            .ToList();
        
        return new Page<Conversation>(conversations, allConversationMessages.PageNumber, allConversationMessages.TotalCount);
    }

    public async Task<Conversation> GetConversationBetweenUsers(Guid currentUserId, IReadOnlySet<Guid> userIds)
    {
        if (userIds.Count < 2)
        {
            throw new ArgumentException("Not enough userIds specified", nameof(userIds));
        }
        
        var conversationId = await _messagingRepository.GetConversationId(userIds);
        if (conversationId == null)
        {
            var participants = await _profileService.Get(new ProfileFilter(idFilter: GuidFilter.WithAnyOf(new List<Guid>(userIds) { currentUserId })));
            return new Conversation(participants.ToHashSet(), new HashSet<ConversationMessage>());
        }
        
        var allMessages = await _messagingRepository.GetConversationMessages(conversationId.Value, PageRequest.All);
        var participantsById = await GetParticipantProfiles(allMessages);

        HashSet<ConversationMessage> messages = new HashSet<ConversationMessage>();
        foreach (var message in allMessages)
        {
            var sender = participantsById[message.SenderId];
            messages.Add(Convert(currentUserId, message, sender));
        }

        await MarkMessagesAsRead(
            allMessages,
            currentUserId,
            DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
        
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

        var allIds = new HashSet<Guid>(recipients) { senderId };
        
        var conversationId = await _messagingRepository.GetConversationId(allIds);
        if (conversationId == null)
        {
            conversationId = await _messagingRepository.CreateConversation();
        }

        var sentAt = DateTime.UtcNow;
        var messageRecipients = new List<MessageRecipientDTO>
        {
            new MessageRecipientDTO(senderId, sentAt)
        };

        foreach (var recipient in recipients)
        {
            messageRecipients.Add(new MessageRecipientDTO(recipient, null));
        }
        
        var message = new MessageDTO(
            Guid.NewGuid(),
            conversationId.Value,
            senderId,
            messageRecipients,
            sentAt,
            body);
        
        await _messagingRepository.CreateMessage(message);

        transactionsScope.Complete();
        return message;
    }
}