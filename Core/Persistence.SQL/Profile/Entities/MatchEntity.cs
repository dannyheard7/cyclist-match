using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.SQL.Profile.Entites;

[Table("match")]
internal class MatchEntity
{
    public MatchEntity(Guid id, Guid sourceUserId, Guid targetUserId, float relevance)
    {
        Id = id;
        SourceUserId = sourceUserId;
        TargetUserId = targetUserId;
        Relevance = relevance;
    }

    [Key]
    public Guid Id { get; init; }
    
    [ForeignKey("user")]
    [Column("source_user_id")]
    public Guid SourceUserId { get; init; }
    
    [ForeignKey("user")]
    [Column("target_user_id")]
    public Guid TargetUserId { get; init; }
    
    public UserEntity SourceUser { get; init; }
    
    public UserEntity TargetUser { get; init; }
    
    public float Relevance { get; set; }
}