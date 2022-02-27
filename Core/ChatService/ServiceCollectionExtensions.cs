using ChatService.ManagedMessaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChatService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddScoped<IChatClient, ChatClient>();
        return serviceCollection;
    }
}