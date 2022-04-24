using Persistence.Profile.Filter;
using Persistence.Profile.Types.DTO;

namespace ProfileService
{
    public interface IProfileService
    {
        public Task<IEnumerable<ProfileDTO>> Get(ProfileFilter filter);
        public Task<ProfileDTO?> GetByExternalId(string externalUserId);
        public Task<ProfileDTO?> GetById(Guid userId);
        public Task Create(CreateProfileDTO profileInput);
        public Task Delete(ProfileDTO profile);
    }
}