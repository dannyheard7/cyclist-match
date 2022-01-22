using System;
using System.Threading.Tasks;
using Auth;
using Persistence.Entity;
using Persistence.Repository;
using Persistence.Types.DTO;

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
        
        public async Task<ProfileDTO> UpsertProfile(ProfileDTO profile)
        {
            throw new NotImplementedException();
            // var result = await _profileRepository.UpsertProfile(profile);
            //
            // if (!result) throw new InvalidOperationException("Error updating profile");
            //
            // return await _profileRepository.GetByUserId(profile.UserId) ??
            //        throw new InvalidOperationException("Profile not found");
        }
    }
}