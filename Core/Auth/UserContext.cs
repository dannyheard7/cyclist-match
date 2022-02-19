using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Auth;

internal class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationUserService _authenticationUserService;

    public UserContext(IHttpContextAccessor httpContextAccessor, IAuthenticationUserService authenticationUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authenticationUserService = authenticationUserService;
    }

    private ClaimsPrincipal? ClaimsPrincipal => _httpContextAccessor.HttpContext?.User;
    
    public string? BearerToken
    {
        get
        {
            if (_httpContextAccessor.HttpContext == null) return null;

            var authHeaders = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (authHeaders.Count == 0) return null;
            if (authHeaders.Count > 1) throw new Exception("Too many authorization headers sent");

            return authHeaders[0];
        }
    }
    
    public async Task<IOIDCUser> GetUser()
    {
        if(BearerToken == null || ClaimsPrincipal == null) throw new UnauthorizedAccessException();

        var oidcUser = await _authenticationUserService.GetUser(ClaimsPrincipal, BearerToken);
        return oidcUser;
    }
}
