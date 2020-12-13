using System;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IUserRepository
    {
        public Task<bool> ExternalUserExists(string externalUserId);
        Task<bool> ExternalUserHasProfile(string externalUserId);
        public Task<IUser?> GetUserDetailsByExternalId(string externalUserId);
        public Task<IUser?> GetUserDetailsByInternalId(Guid internalUserId);
        public Task<bool> UpdateUserDetails(IUser user);
        public Task DeleteUser(IUser user);
    }
}