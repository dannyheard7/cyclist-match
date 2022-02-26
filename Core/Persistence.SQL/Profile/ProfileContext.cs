using Microsoft.EntityFrameworkCore;
using Persistence.SQL.Profile.Entites;

namespace Persistence.SQL;

internal class ProfileContext : DbContext
{
    public ProfileContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; init; }
    
    public DbSet<ProfileEntity> Profiles { get; init; }
    
    public DbSet<MatchEntity> ProfileMatches { get; init; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}