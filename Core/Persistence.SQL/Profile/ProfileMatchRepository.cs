using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Profile;
using Persistence.Profile.Types.DTO;
using Persistence.SQL.Profile.Entites;
using Persistence.SQL.Profile.Mapper;


namespace Persistence.SQL.Profile;

internal class ProfileMatchRepository : IProfileMatchRepository
{
    private readonly ProfileContext _context;

    public ProfileMatchRepository(ProfileContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProfileDTO>> GetMatchedProfiles(ProfileDTO profile)
    {
        var results = await _context.ProfileMatches
            .Where(x => x.SourceUserId == profile.UserId)
            .Include(x => x.TargetUser)
            .ThenInclude(x => x.Profile)
            .OrderByDescending(x => x.Relevance)
            .AsNoTracking()
            .ToListAsync();

        return results.Select(x => x.TargetUser.Profile.Map());
    }
    
    public async Task UpdateProfileMatches(ProfileDTO profileDto, IReadOnlyCollection<MatchDTO> matches)
    {
        _context.RemoveRange(_context.ProfileMatches.Where(x => x.SourceUserId == profileDto.UserId));
        await _context.ProfileMatches
            .AddRangeAsync(matches
                .Select(m => new MatchEntity(Guid.NewGuid(), m.SourceProfile.UserId, m.TargetProfile.UserId, m.RelevanceScore)));

        await _context.SaveChangesAsync();
    }

   
}