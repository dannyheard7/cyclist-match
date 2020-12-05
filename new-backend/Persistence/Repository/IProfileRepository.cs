using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entity;
using RuntimeService.DTO;

namespace Persistence.Repository
{
    public interface IProfileRepository
    {
        public Task<Profile?> GetByUserId(Guid userId);

        public Task<bool> UpsertProfile(Profile profile);
        
        public Task<IEnumerable<ProfileMatch>> GetMatchingProfiles(IUser user);
    }
}