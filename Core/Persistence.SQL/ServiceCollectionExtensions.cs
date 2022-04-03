using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Messaging;
using Persistence.Profile;
using Persistence.SQL.Messaging;
using Persistence.SQL.Migrations;
using Persistence.SQL.Profile;

namespace Persistence.SQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CyclingBuddiesContext");
            
            services
                .AddSingleton<ConnectionFactory>(_ => new ConnectionFactory(connectionString))
                .AddSingleton<IDatabaseMigrator, DatabaseMigrator>()
                .AddHostedService<DatabaseMigratorHostedService>();
            
            services
                .AddDbContext<PersistenceContext>(options => options
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseNpgsql(
                        connectionString, 
                        o =>
                        {
                            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            o.UseNetTopologySuite();
                        }));

            services.AddScoped<IMessagingRepository, MessagingRepository>();

            return services
                .AddScoped<IProfileRepository, ProfileRepository>()
                .AddScoped<IProfileMatchRepository, ProfileMatchRepository>();
        }

        public static IGlobalConfiguration UseHangfirePersistence(this IGlobalConfiguration hangfireConfiguration, IConfiguration configuration)
        {
            hangfireConfiguration.UsePostgreSqlStorage(configuration.GetConnectionString("Hangfire"));
            return hangfireConfiguration;
        }
    }
}