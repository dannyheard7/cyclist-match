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
    }
}