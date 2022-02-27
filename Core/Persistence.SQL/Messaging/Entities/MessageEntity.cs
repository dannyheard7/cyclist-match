using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persistence.SQL.Profile.Entites;

namespace Persistence.SQL.Messaging.Entities;

[Table("message")]
internal class MessageEntity
{
    [Key]
    public Guid Id { get; init; }
    
    public string Body { get; init; }
    
    public DateTime SentAt { get; init; }
    
    [ForeignKey("sender")]
    public Guid SenderId { get; init; }
    
    [ForeignKey("conversation")]
    public Guid ConversationId { get; init; }
    
    public UserEntity Sender { get; init; }
    
    public IReadOnlyCollection<MessageRecipient> Recipients { get; init; }
    
    public ConversationEntity Conversation { get; init; }
}