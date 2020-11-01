using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            Auth0Settings settings = new Auth0Settings();
            configuration.GetSection("Auth0").Bind(settings);
            services.Configure<Auth0Settings>(options => configuration.GetSection("Auth0").Bind(options));
            
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
                .AddScoped<IExternalUserService, Auth0UserService>();
        }
    }
}