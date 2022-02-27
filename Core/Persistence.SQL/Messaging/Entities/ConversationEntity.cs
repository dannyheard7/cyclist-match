using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.SQL.Messaging.Entities;

[Table("conversation")]
internal class ConversationEntity
{
    public Guid Id { get; init; }
    
    public IReadOnlyCollection<MessageEntity> Messages { get; }
}