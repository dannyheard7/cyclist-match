using Auth;
using Microsoft.Extensions.Options;

namespace ChatService;

internal class MatrixClientFactory : IChatClientFactory
{
    private readonly IUserContext _userContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _host;

    public MatrixClientFactory(
        IUserContext userContext,
        IHttpClientFactory httpClientFactory,
        IOptions<ChatServiceSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.Host))
        {
            throw new InvalidOperationException($"{ChatServiceSettings.Key}:{nameof(settings.Value.Host)} is not a valid URI");
        }

        _userContext = userContext;
        _httpClientFactory = httpClientFactory;
        _host = new Uri(settings.Value.Host);
    }

    public IChatClient GetClient()
    {
        return new MatrixClient(_userContext, _httpClientFactory, _host);
    }
}