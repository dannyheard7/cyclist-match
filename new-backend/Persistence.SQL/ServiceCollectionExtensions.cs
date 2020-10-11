using Microsoft.Extensions.DependencyInjection;

namespace Persistence.SQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services
                .AddSingleton<ConnectionFactory>();
        }
    }
}