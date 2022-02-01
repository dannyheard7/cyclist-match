using Microsoft.Extensions.DependencyInjection;

namespace MatchingService;

public static class ServiceCollectionExtension
{
    public static void AddMatchingService(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IRelevanceCalculator, BasicRelevanceCalculator>()
            .AddScoped<IMatchingService, MatchingService>();
    }
}