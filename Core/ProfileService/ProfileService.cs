using Persistence.Profile;
using Persistence.Profile.Filter;
using Persistence.Profile.Types.DTO;

namespace ProfileService
{
    internal class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
        }

        public Task<IEnumerable<ProfileDTO>> Get(ProfileFilter filter) => _profileRepository.Get(filter);

        public Task<ProfileDTO?> GetByExternalId(string externalUserId) =>
            _profileRepository.GetByExternalUserId(externalUserId);

        public Task<ProfileDTO?> GetById(Guid userId) =>  _profileRepository.GetByUserId(userId);
        
        public async Task Create(CreateProfileDTO profile)
        {
            await _profileRepository.Create(profile);
            
            // Once a profile has been created or updated we queue a hangfire job that finds relevant matches
        }
    }
}