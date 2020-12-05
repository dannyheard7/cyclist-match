using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entity;
using Persistence.Objects;

namespace Persistence.Repository
{
    public interface IProfileRepository
    {
        public Task<Profile?> GetByUserId(Guid userId);

        public Task<bool> UpsertProfile(Profile profile);
        
        public Task<IEnumerable<MatchingProfile>> GetMatchingProfiles(IUser user);
    }
}