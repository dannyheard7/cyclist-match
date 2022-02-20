using System.Text.Json;
using Auth;
using ChatService.Models;
using Microsoft.Net.Http.Headers;

namespace ChatService;

internal class MatrixClient : IChatClient
{
    private const string HttpClientName = "Matrix";
        
    private readonly IUserContext _userContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _host;

    public MatrixClient(
        IUserContext userContext,
        IHttpClientFactory httpClientFactory,
        Uri host)
    {
        _userContext = userContext;
        _httpClientFactory = httpClientFactory;
        _host = host;
    }

    private HttpRequestMessage BuildRequestMessage(HttpMethod method, string relativePath)
    {
        var authHeader = _userContext.BearerToken;

        return new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(_host, relativePath),
            Headers =
            {
                { HeaderNames.Authorization, _userContext.BearerToken }
            }
        };
    }

    private Task<HttpResponseMessage> Send(HttpRequestMessage request)
    {
        var client = _httpClientFactory.CreateClient(HttpClientName);
        return client.SendAsync(request);
    }

    public async Task<IReadOnlyCollection<Conversation>> GetConversations()
    {
        var requestMessage = BuildRequestMessage(HttpMethod.Get, "joined_rooms");
        
        var response = await Send(requestMessage);
        var content = await response.Content.ReadAsStringAsync();
        var inbox = await JsonSerializer.DeserializeAsync<JoinedRooms>(await response.Content.ReadAsStreamAsync());

        return new List<Conversation>();
    }
}