using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Types.DTO;

namespace Persistence.Repository;

public interface IProfileMatchRepository
{
    Task<IEnumerable<ProfileDTO>> GetMatchedProfiles(ProfileDTO profile);
    
    Task UpdateProfileMatches(ProfileDTO profile, IReadOnlyCollection<MatchDTO> matches);
}