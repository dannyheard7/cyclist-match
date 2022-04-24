using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Profile.Filter;
using Persistence.Profile.Types.DTO;

namespace Persistence.Profile
{
    public interface IProfileRepository
    {
        public Task<ProfileDTO?> GetByExternalUserId(string externalUserId);
        
        public Task<ProfileDTO?> GetByUserId(Guid userId);

        public Task Create(CreateProfileDTO profile);
        
        public Task<IEnumerable<ProfileDTO>> Get(ProfileFilter filter);

        public Task Delete(ProfileDTO profile);
    }
}