using System;
using Auth;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Repository;
using Persistence.SQL;
using RuntimeService.Services;

namespace RuntimeService
{
    public class Startup
    {
        readonly string AllowSpecificOriginsCors = "_AllowSpecificOrigins";
        private readonly string CORSOriginsVariable = "CORS_ORIGINS";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            if(String.IsNullOrEmpty(Configuration[CORSOriginsVariable]))
                throw new ArgumentNullException(CORSOriginsVariable);
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOriginsCors,
                    builder =>
                    {
                        builder
                            .WithOrigins(Configuration[CORSOriginsVariable].Split(";"))
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            
            services.AddPersistence();
            services.AddAuth(Configuration);

            services
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IProfileMatchService, ProfileMatchService>()
                .AddScoped<IMessageService, MessageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseCors(AllowSpecificOriginsCors);
            app.UseRouting();

            app
                .UseAuthentication()
                .UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}