using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Persistence.Entity;
using Persistence.Filter;
using Persistence.Repository;
using Persistence.SQL.Entities;
using Persistence.SQL.Filters;
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

        public async Task Create(CreateProfileDTO profile)
        {
            await _context.Profiles.AddAsync(new ProfileEntity
            {
                UserId = profile.UserId,
                AverageDistance = profile.AverageDistance,
                AverageSpeed = profile.AverageSpeed,
                Availability = new List<Availability>(profile.Availability),
                CyclingTypes = new List<CyclingType>(profile.CyclingTypes),
                Location = LocationMapper.Map(profile.Location),
                User = new UserEntity
                {
                    Id = profile.UserId,
                    ExternalId = profile.ExternalUserId,
                    DisplayName = profile.UserDisplayName,
                    Picture = profile.UserPicture,
                    CreatedAt = profile.CreatedAt,
                    UpdatedAt = profile.UpdatedAt
                }
            });
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProfileDTO>> Get(ProfileFilter filter)
        {
            var dbSet = _context.Profiles.AsQueryable();

            if (filter.IdFilter != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.UserId), filter.IdFilter);
            }

            if (filter.Availability != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.Availability), filter.Availability);
            }
            
            if (filter.CyclingTypes != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.CyclingTypes), filter.CyclingTypes);
            }

            if (filter.AverageDistanceFilter != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.AverageDistance), filter.AverageDistanceFilter);
            }
            
            if (filter.AverageSpeedFilter != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.AverageSpeed), filter.AverageSpeedFilter);
            }

            if (filter.LocationFilter != null)
            {
                dbSet = dbSet.ApplyFilter(nameof(ProfileEntity.Location), filter.LocationFilter);
            }

            var results = await dbSet.Include(x => x.User).AsNoTracking().ToListAsync();
            return results.Select(ProfileMapper.Map);
        }

       
    }
}