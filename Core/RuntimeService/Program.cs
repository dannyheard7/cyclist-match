using System;
using System.Globalization;
using Auth;
using ChatService;
using Hangfire;
using MatchingService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.SQL;
using RuntimeService.Services;
using RuntimeService.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services
    .AddHttpContextAccessor()
    .AddHttpClient();

builder.Services
    .AddPersistence(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddScoped<IProfileService, ProfileService>();

builder.Services
    .AddChatService(builder.Configuration)
    .AddMatchingService();

builder.Services.Configure<ClientConfigSettings>(builder.Configuration.GetSection(ClientConfigSettings.Key));

builder.Services.AddHangfire(configuration =>
    {
        configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseHangfirePersistence(builder.Configuration);
    })
    .AddHangfireServer();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".js") || ctx.File.Name.EndsWith(".css"))
        {
            ctx.Context.Response.Headers.CacheControl = "public";
            ctx.Context.Response.Headers.Expires = DateTime.UtcNow.AddYears(1).ToString("R", CultureInfo.InvariantCulture);
        }
    }
});
app.UseRouting();

app
    .UseAuthentication()
    .UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
    endpoints.MapHealthChecks("/healthz");
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
