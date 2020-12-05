using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Persistence.Repository;
using RuntimeService.DTO;

namespace RuntimeService.Services
{
    public class ProfileMatchService : IProfileMatchService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileMatchService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<IEnumerable<ProfileMatch>> GetProfileMatches(IUser user, int maxResults=15, int? page=null)
        {
            var matchingProfiles = await _profileRepository.GetMatchingProfiles(user);
            return matchingProfiles.Select(mp => new ProfileMatch()
            {
                UserId = mp.UserId,
                DisplayName = mp.DisplayName,
                LocationName = mp.LocationName
            });
        }
    }
}