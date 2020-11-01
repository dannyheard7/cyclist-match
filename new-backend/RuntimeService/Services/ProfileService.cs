using System;
using System.Threading.Tasks;
using Persistence.Entity;
using Persistence.Repository;

namespace RuntimeService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
        }

        public Task<Profile?> Get(Guid userId) =>  _profileRepository.GetByUserId(userId);
        
        public async Task<Profile> UpsertProfile(Profile profile)
        {
            var result = await _profileRepository.UpsertProfile(profile);

            if (!result) throw new InvalidOperationException("Error updating profile");
            
            return await _profileRepository.GetByUserId(profile.UserId) ??
                   throw new InvalidOperationException("Profile not found");
        }
    }
}