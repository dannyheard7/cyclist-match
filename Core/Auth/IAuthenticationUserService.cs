using System.Security.Claims;
using System.Threading.Tasks;
using Persistence;

namespace Auth
{
    public interface IAuthenticationUserService
    {
        Task<string> GetExternalUserId(ClaimsPrincipal claimsPrincipal);
        public Task<IOIDCUser> GetUser(ClaimsPrincipal claimsPrincipal, string bearerToken);
    }
}