using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Profile.Types.DTO;

namespace Persistence.Profile;

public interface IProfileMatchRepository
{
    Task<IEnumerable<ProfileDTO>> GetMatchedProfiles(ProfileDTO profile);
    
    Task UpdateProfileMatches(ProfileDTO profile, IReadOnlyCollection<MatchDTO> matches);
}