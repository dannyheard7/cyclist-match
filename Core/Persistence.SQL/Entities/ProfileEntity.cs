using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Persistence.Entity;

namespace Persistence.SQL.Entities;

[Table("user_cycling_profile", Schema = "cycling")]
internal class ProfileEntity
{
    [Key]
    [ForeignKey("user")]
    public Guid UserId { get; init; }

    public Point Location { get; set; }
    
    public ICollection<CyclingType> CyclingTypes { get; set; }
    
    public ICollection<Availability> Availability { get; set; }
    
    public int AverageDistance { get; set; }
    
    public int AverageSpeed { get; set; }
    
    public UserEntity User { get; init; }
    
    public DateTime CreatedAt { get; init; }
        
    public DateTime UpdatedAt { get; set; }
}