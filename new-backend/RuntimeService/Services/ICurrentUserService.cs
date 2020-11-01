using System.Security.Claims;
using System.Threading.Tasks;
using Persistence;

namespace Auth
{
    public interface ICurrentUserService
    {
        Task<string> GetExternalUserId();
        public Task<IUser> GetUser();
    }
}