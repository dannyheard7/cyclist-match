using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Persistence;

namespace Auth
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<Auth0Settings> _options;
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IOptions<Auth0Settings> options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
    
        public Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null) throw new UnauthorizedAccessException();
        
            return Task.FromResult(user);
        }
        
        public async Task<string> GetExternalUserId()
        {
            var claimsPrincipal = await GetClaimsPrincipal();
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IUser> GetUser()
        {
            if (_httpContextAccessor.HttpContext == null) throw new UnauthorizedAccessException();
            
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization][0];

            var httpClient = _httpClientFactory.CreateClient();
                
            using var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri(new Uri(_options.Value.Domain), "userinfo"),
                Headers = {
                    { HeaderNames.Authorization, accessToken},
                }
            };
                
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<Auth0User>(await response.Content.ReadAsStringAsync());
        }
    }
}