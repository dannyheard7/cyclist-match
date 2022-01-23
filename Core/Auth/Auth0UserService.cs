using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Persistence;

namespace Auth
{
    internal class Auth0UserService : IAuthenticationUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OidcSettings _settings;
        
        public Auth0UserService(IHttpClientFactory httpClientFactory, IOptions<OidcSettings> options)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<string> GetExternalUserId(ClaimsPrincipal claimsPrincipal)
        {
            return Task.FromResult(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public async Task<IOIDCUser> GetUser(ClaimsPrincipal claimsPrincipal, string bearerToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
                
            using var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri(new Uri(_settings.Domain), _settings.UserInfoEndpoint),
                Headers = {
                    { HeaderNames.Authorization, bearerToken},
                }
            };
                
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<Auth0User>(await response.Content.ReadAsStringAsync());
        }
    }
}