using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persistence.SQL.Profile.Entites;

namespace Persistence.SQL.Messaging.Entities;

[Table("message_recipient")]
internal class MessageRecipient
{
    [Key]
    public Guid Id { get; init; }

    [ForeignKey("recipient")]
    public Guid RecipientId { get; init; }
    
    [ForeignKey("message")]
    public Guid MessageId { get; init; }
    
    public DateTime? ReadAt { get; set; }

    [NotMapped]
    public bool Read => ReadAt != null;


    public MessageEntity Message { get; init; }
    
    public UserEntity Recipient { get; init; }
}