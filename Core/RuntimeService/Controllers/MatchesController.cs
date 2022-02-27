using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Profile.Types.DTO;
using RuntimeService.Services;

namespace RuntimeService.Controllers;

[ApiController]
[Route("api/matches")]
[Authorize]
public class MatchesController : Controller
{
    private readonly IUserContext _userContext;
    private readonly IMatchingService _matchingService;
    
    public class MatchesResponse
    {
        public MatchesResponse(IEnumerable<ProfileDTO> matches)
        {
            Matches = matches;
        }

        public IEnumerable<ProfileDTO> Matches { get; }
    }

    public MatchesController(IUserContext userContext, IMatchingService matchingService)
    {
        _userContext = userContext;
        _matchingService = matchingService;
    }

    [HttpGet]
    public async Task<ActionResult<MatchesResponse>> GetProfileMatches()
    {
        var profile = await _userContext.GetUserProfile();
        if (profile == null) return NotFound();

        var matches = await _matchingService.GetMatchedProfiles(profile);
        return Ok(new MatchesResponse(matches));
    }
}