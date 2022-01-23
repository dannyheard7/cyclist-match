using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Persistence.Entity;

namespace Persistence.SQL.Entities;

[Table("user_cycling_profile")]
internal class ProfileEntity
{
    [Key]
    [ForeignKey("user")]
    public Guid UserId { get; init; }

    public Point Location { get; set; }
    
    [Column(TypeName = "VARCHAR(30)[]")]
    public List<CyclingType> CyclingTypes { get; set; }
    
    [Column(TypeName = "VARCHAR(30)[]")]
    public List<Availability> Availability { get; set; }
    
    public int AverageDistance { get; set; }
    
    public int AverageSpeed { get; set; }
    
    public UserEntity User { get; init; }
}