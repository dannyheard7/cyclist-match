using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Repository;
using Persistence.SQL.Mapper;
using Persistence.Types.DTO;

namespace Persistence.SQL.Repository
{
    internal class ProfileRepository : IProfileRepository
    {
        private readonly CyclingBuddiesContext _context;

        public ProfileRepository(CyclingBuddiesContext context)
        {
            _context = context;
        }

        public async Task<ProfileDTO?> GetByExternalUserId(string externalUserId)
        {
            var result = await _context.Profiles
                .Where(x => x.User.ExternalId == externalUserId)
                .Include(x => x.User)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            return result?.Map();
        }

        public async Task<ProfileDTO?> GetByUserId(Guid userId)
        {
            var result = await _context.Profiles
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            return result?.Map();
        }

        public Task UpdateProfile(ProfileDTO profile)
        {
            throw new NotImplementedException();
        }
    }
}