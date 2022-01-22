using Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.SQL;
using RuntimeService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
            
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);

builder.Services
    .AddScoped<IProfileService, ProfileService>()
    .AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app
    .UseAuthentication()
    .UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


app.Run();
