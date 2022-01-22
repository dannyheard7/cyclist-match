using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entity;
using Persistence.Types.DTO;

namespace Persistence.Repository
{
    public interface IProfileRepository
    {
        public Task<ProfileDTO?> GetByExternalUserId(string externalUserId);
        
        public Task<ProfileDTO?> GetByUserId(Guid userId);

        public Task UpdateProfile(ProfileDTO profile);
        
        // public Task<IEnumerable<ProfileDTO>> GetMatchingProfiles(IUser user);
    }
}