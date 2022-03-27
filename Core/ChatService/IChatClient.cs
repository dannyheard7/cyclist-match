﻿using ChatService.Models;
using Common;
using Persistence.Messaging.Types;

namespace ChatService;

public interface IChatClient
{
    public Task<Page<Conversation>> GetUserConversations(Guid userId, PageRequest? pageRequest);

    Task<Conversation?> GetConversationBetweenUsers(IReadOnlySet<Guid> userIds);

    public Task<MessageDTO> SendMessage(Guid senderId, IReadOnlySet<Guid> recipients, string body);
}