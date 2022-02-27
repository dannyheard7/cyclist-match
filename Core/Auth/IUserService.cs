using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth
{
    public interface IUserService
    {
        Task<string> GetExternalUserId(ClaimsPrincipal claimsPrincipal);
        public Task<IOIDCUser> GetUser(ClaimsPrincipal claimsPrincipal, string bearerToken);
    }
}