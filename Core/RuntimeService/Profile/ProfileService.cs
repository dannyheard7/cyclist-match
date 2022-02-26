using System;
using System.Threading.Tasks;
using Auth;
using Persistence.Profile;
using Persistence.Profile.Types.DTO;

namespace RuntimeService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
        }

        public Task<ProfileDTO?> GetByOidcUser(IOIDCUser oidcUser) =>
            _profileRepository.GetByExternalUserId(oidcUser.Id);

        public Task<ProfileDTO?> GetById(Guid userId) =>  _profileRepository.GetByUserId(userId);
        
        public async Task Create(CreateProfileDTO profile)
        {
            await _profileRepository.Create(profile);
            
            // Once a profile has been created or updated we queue a hangfire job that finds relevant matches
        }
    }
}