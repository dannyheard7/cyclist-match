using Hangfire;
using MatchingService;
using Persistence.Profile;
using Persistence.Profile.Filter;
using Persistence.Profile.Types.DTO;

namespace ProfileService
{
    internal class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ProfileService(IProfileRepository profileRepository, IBackgroundJobClient backgroundJobClient)
        {
            _profileRepository = profileRepository;
            _backgroundJobClient = backgroundJobClient;
        }

        public Task<IEnumerable<ProfileDTO>> Get(ProfileFilter filter) => _profileRepository.Get(filter);

        public Task<ProfileDTO?> GetByExternalId(string externalUserId) =>
            _profileRepository.GetByExternalUserId(externalUserId);

        public Task<ProfileDTO?> GetById(Guid userId) =>  _profileRepository.GetByUserId(userId);
        
        public async Task Create(CreateProfileDTO profile)
        {
            await _profileRepository.Create(profile);
            _backgroundJobClient.Enqueue<IMatchingService>(service => service.MatchRelevantProfiles(profile.UserId));
        }

        public async Task Delete(ProfileDTO profile)
        {
            // TODO: does this need to orchestrate deleting messages, matches etc.
            // Maybe it should be a background job?
            await _profileRepository.Delete(profile);
        }
    }
}