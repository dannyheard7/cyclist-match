using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using Persistence.SQL.Repository;

namespace Persistence.SQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services
                .AddSingleton<ConnectionFactory>()
                .AddScoped<IProfileRepository, ProfileRepository>()
                .AddScoped<IUserRepository, UserRepository>();
        }
    }
}