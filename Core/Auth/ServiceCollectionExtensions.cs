using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            Auth0Settings settings = configuration.GetSection(Auth0Settings.Key).Get<Auth0Settings>();
            services.Configure<Auth0Settings>(_ => Options.Create(settings));
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = settings.Domain;
                    options.Audience = settings.Audience;
                });
            return services
                .AddScoped<IAuthenticationUserService, Auth0UserService>();
        }
    }
}