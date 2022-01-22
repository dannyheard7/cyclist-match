using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
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
                            o.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
                            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        }));

            return services
                .AddScoped<IProfileRepository, ProfileRepository>();
        }
    }
}