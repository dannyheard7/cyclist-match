using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
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

    public async Task<Page<MessageDTO>> GetUserConversations(Guid userId, PageRequest pageRequest, bool unreadOnly)
    {
        var query =
            _context
                .Conversations
                .Include(x => x.Messages)
                .ThenInclude(x => x.Recipients)
                .Where(x => x.Messages.Any(y =>
                    y.Recipients.Any(z => z.RecipientId == userId && (!unreadOnly || z.ReadAt == null))))
                .Select(x => x.Messages.OrderByDescending(y => y.SentAt).FirstOrDefault())
             .Where(x => x != null) as IQueryable<MessageEntity>;
        
        var messages = await 
            query
            .OrderByDescending(x => x!.SentAt)
            .Skip(pageRequest.Skip)
            .Take(pageRequest.PageSize)
            .ToListAsync();
        
        return new Page<MessageDTO>(
            messages.Select(x => x!.Map()).ToList(),
            pageRequest.Page,
            await query.CountAsync());
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
 
    public async Task<IReadOnlyCollection<MessageDTO>> GetConversationMessages(Guid conversationId, PageRequest pageRequest)
    {
        var messages = await
            _context
                .Messages
                .Include(x => x.Recipients)
                .Where(x => x.ConversationId == conversationId)
                .OrderByDescending(x => x.SentAt)
                .Skip(pageRequest.Skip)
                .Take(pageRequest.PageSize)
                .ToListAsync();

        return messages.Select(x => x.Map()).Reverse().ToList();
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

    public async Task CreateMessage(MessageDTO messageDto)
    { 
        var message = new MessageEntity
        {
            Id = messageDto.Id,
            Body = messageDto.Body,
            SenderId = messageDto.SenderId,
            SentAt = messageDto.SentAt,
            ConversationId = messageDto.ConversationId,
            Recipients = messageDto
                .Recipients
                .Select(mr => new MessageRecipient
                {
                    Id = Guid.NewGuid(),
                    RecipientId = mr.RecipientId,
                    ReadAt = mr.ReadAt
                })
                .ToList()
        };

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMessage(MessageDTO messageDto)
    {
        var existingMessage = await _context.Messages
            .Include(x => x.Recipients)
            .SingleAsync(x => x.Id == messageDto.Id);

        // The only thing we update is the readAt time
        foreach (var recipient in existingMessage.Recipients)
        {
            var updatedRecipient = messageDto.Recipients.Single(x => x.RecipientId == recipient.RecipientId);
            recipient.ReadAt = updatedRecipient.ReadAt;

            _context.Entry(recipient).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
    }
}