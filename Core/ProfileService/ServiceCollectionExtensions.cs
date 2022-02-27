using Microsoft.Extensions.DependencyInjection;

namespace ProfileService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileService(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IProfileService, ProfileService>();

        return serviceCollection;
    }
}