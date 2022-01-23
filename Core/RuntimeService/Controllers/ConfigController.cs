using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RuntimeService.Settings;

namespace RuntimeService.Controllers;

[ApiController]
[Route("/config")]
public class ConfigController
{
    private readonly ClientConfigSettings _settings;

    public ConfigController(IOptions<ClientConfigSettings> options)
    {
        _settings = options.Value;
    }

    [HttpGet]
    public ClientConfigSettings GetConfig()
    {
        return _settings;
    }
}