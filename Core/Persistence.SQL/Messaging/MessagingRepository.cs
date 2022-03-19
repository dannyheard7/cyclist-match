using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Messaging;
using Persistence.Messaging.Types;
using Persistence.SQL.Messaging.Entities;
using Persistence.SQL.Messaging.Mapper;

namespace Persistence.SQL.Messaging;

internal class MessagingRepository : IMessagingRepository
{
    private readonly PersistenceContext _context;

    public MessagingRepository(PersistenceContext context)
    {
        _context = context;
    }

    public async Task<Guid?> GetConversationId(IReadOnlyCollection<Guid> userIds)
    {
        var userIdsAsList = userIds.ToList();
        return await _context
            .ConversationParticipantsAggregates
            .Where(x => 
                x.Participants.All(i => userIdsAsList.Contains(i)) &&
                userIdsAsList.All(i => x.Participants.Contains(i)))
            .Select(x => x.ConversationId as Guid?)
            .FirstOrDefaultAsync();
    }
 
    public async Task<IReadOnlyCollection<MessageDTO>> GetConversationMessages(Guid conversationId)
    {
        var messages = await
            _context
                .Messages
                .Include(x => x.Recipients)
                .Where(x => x.ConversationId == conversationId)
                .OrderByDescending(x => x.SentAt)
                .AsNoTracking()
                .ToListAsync();

        return messages.Select(x => x.Map()).ToList();
    }

    public async Task<Guid> CreateConversation()
    {
        var id = Guid.NewGuid();
        await _context.Conversations.AddAsync(new ConversationEntity
        {
            Id = id
        });
        await _context.SaveChangesAsync();

        return id;
    }

    public async Task CreateMessages(IReadOnlyCollection<MessageDTO> messageDtos)
    {
        var messages = messageDtos
            .GroupBy(x => new
            {
                Id = x.Id,
                SenderId = x.SenderId,
                Body = x.Body,
                SentAt = x.SentAt,
                ConversationId = x.ConversationId
            })
            .Select(message =>
            {
                return new MessageEntity
                {
                    Id = message.Key.Id,
                    Body = message.Key.Body,
                    SenderId = message.Key.SenderId,
                    SentAt = message.Key.SentAt,
                    ConversationId = message.Key.ConversationId,
                    Recipients = message
                        .Select(mr => new MessageRecipient
                        {
                            Id = Guid.NewGuid(),
                            RecipientId = mr.RecipientId,
                            ReadAt = mr.ReadAt
                        })
                        .ToList()
                };
            });

        await _context.Messages.AddRangeAsync(messages);
        await _context.SaveChangesAsync();
    }
}