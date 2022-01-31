using Persistence.Types.DTO;

namespace MatchingService;

public interface IMatchingService
{
    public Task MatchRelevantProfiles(Guid profileId);
}