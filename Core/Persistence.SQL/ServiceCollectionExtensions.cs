using System;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using Persistence.SQL.Migrations;
using Persistence.SQL.Repository;

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
                .AddDbContext<CyclingBuddiesContext>(options => options
                    .UseNpgsql(
                        connectionString, 
                        o =>
                        {
                            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            o.UseNetTopologySuite();
                        }));

            return services
                .AddScoped<IProfileRepository, ProfileRepository>();
        }

        public static IGlobalConfiguration UseHangfirePersistence(this IGlobalConfiguration hangfireConfiguration, IConfiguration configuration)
        {
            hangfireConfiguration.UsePostgreSqlStorage(configuration.GetConnectionString("Hangfire"));
            return hangfireConfiguration;
        }
    }
}