using System;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IUserRepository
    {
        Task<bool> ExternalUserHasProfile(string externalUserId);
        public Task<IUser> GetUserDetails(string externalUserId);
        public Task<bool> UpdateUserDetails(IUser user);
    }
}