using Microsoft.EntityFrameworkCore;
using Persistence.SQL.Messaging.Entities;
using Persistence.SQL.Profile.Entites;

namespace Persistence.SQL;

internal class PersistenceContext : DbContext
{
    public PersistenceContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    public DbSet<ProfileEntity> Profiles { get; init; }
    
    public DbSet<MatchEntity> ProfileMatches { get; init; }
    
    public DbSet<ConversationEntity> Conversations { get; init; }
    public DbSet<ConversationParticipantsAggregate> ConversationParticipantsAggregates { get; init; }

    public DbSet<MessageEntity> Messages { get; init; }
}