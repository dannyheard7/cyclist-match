using System.Security.Claims;
using System.Threading.Tasks;
using Persistence;

namespace Auth
{
    public interface IExternalUserService
    {
        Task<string> GetExternalUserId(ClaimsPrincipal claimsPrincipal);
        public Task<IUser> GetUser(ClaimsPrincipal claimsPrincipal, string bearerToken);
    }
}