using System.Security.Claims;
using System.Threading.Tasks;
using Persistence;

namespace Auth
{
    public interface ICurrentUserService
    {
        public Task<ClaimsPrincipal> GetClaimsPrincipal();
        Task<string> GetExternalUserId();
        public Task<IUser> GetUser();
    }
}