using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Persistence;
using Persistence.Repository;

namespace RuntimeService.Services
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IExternalUserService _externalUserService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IExternalUserService externalUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _externalUserService = externalUserService;
        }

        private ClaimsPrincipal? ClaimsPrincipal => _httpContextAccessor.HttpContext?.User;
        private string? BearerToken
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

        public async Task<string> GetExternalUserId()
        {
            var claimsPrincipal = ClaimsPrincipal;
            if(claimsPrincipal == null) throw new UnauthorizedAccessException();
            
            return await _externalUserService.GetExternalUserId(claimsPrincipal);
        }

        public async Task<IUser> GetUser()
        {
            var externalUserId = await GetExternalUserId();
            
            var user = await _userRepository.GetUserDetails(externalUserId);
            if (user != null) return user;
            
            var bearerToken = BearerToken;
            var claimsPrincipal = ClaimsPrincipal;
            if(bearerToken == null || claimsPrincipal == null) throw new UnauthorizedAccessException();

            var externalUser = await _externalUserService.GetUser(claimsPrincipal, bearerToken);
            await _userRepository.UpdateUserDetails(externalUser);
            return externalUser;
        }
    }
}