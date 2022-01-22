using Microsoft.EntityFrameworkCore;
using Persistence.SQL.Entities;

namespace Persistence.SQL;

internal class CyclingBuddiesContext : DbContext
{
    public CyclingBuddiesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; }
    
    public DbSet<ProfileEntity> Profiles { get; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSnakeCaseNamingConvention();
}