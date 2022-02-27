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
            var settingsSection = configuration.GetSection(OidcSettings.Key);
            var oidcSettings = settingsSection.Get<OidcSettings>();
            services.Configure<OidcSettings>(settingsSection);
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = oidcSettings.Host;
                    options.Audience = oidcSettings.Audience;
                });
            
            return services
                .AddSingleton<IUserService, Auth0Service>();
        }
    }
}