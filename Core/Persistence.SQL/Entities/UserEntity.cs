using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.SQL.Entities;

[Table("user")]
internal class UserEntity
{
    [Key]
    public Guid Id { get; init; }
    
    public string ExternalId { get; init; }
    
    public string? Picture { get; set; }
    
    public string DisplayName { get; set; }
    
    public DateTime CreatedAt { get; init; }
        
    public DateTime UpdatedAt { get; set; }
    
    public ProfileEntity Profile { get; init; }
}