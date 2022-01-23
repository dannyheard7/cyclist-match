using Microsoft.EntityFrameworkCore;
using Persistence.SQL.Entities;

namespace Persistence.SQL;

internal class CyclingBuddiesContext : DbContext
{
    public CyclingBuddiesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; init; }
    
    public DbSet<ProfileEntity> Profiles { get; init; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSnakeCaseNamingConvention();
}