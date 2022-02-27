using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SQL.Messaging.Entities;

[Table("conversation_participants_aggregate")]
[Keyless]
internal class ConversationParticipantsAggregate
{
    public Guid ConversationId { get; private init; }
    
    [Column("participants", TypeName = "uuid[]")]
    public List<Guid> Participants { get; private init; }
}