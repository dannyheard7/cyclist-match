using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Persistence.Entity;
using Persistence.Repository;

namespace Persistence.SQL.Repository
{
    internal class ProfileRepository : IProfileRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        
        public ProfileRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }
        
        public async Task<bool> HasProfile(Guid userId)
        {
            await using var connection = _connectionFactory.Create();
            return await connection.QueryFirstOrDefaultAsync<bool>(
                "SELECT EXISTS(SELECT 1 FROM user_profile WHERE user_id=@UserId)",
                new
                {
                    UserId = userId
                }
            );
        }

        private class ProfileQueryResult
        {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
            public string LocationName { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public ICollection<string> CyclingTypes { get; set; }
            public ICollection<string> Availability { get; set; }
            public int MinDistance { get; set; }
            public int MaxDistance { get; set; }
            public int Speed { get; set;  }
        
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            public Profile ToProfile()
            {
                var location = new Location(Longitude, Latitude);
                var cyclingTypes = CyclingTypes.Select(value =>
                {
                    Enum.TryParse(value, out CyclingType type);
                    return type;
                }).ToList();
                var availability = Availability.Select(value =>
                {
                    Enum.TryParse(value, out AvailabilityItem type);
                    return type;
                }).ToList();
                return new Profile(UserId, DisplayName, LocationName, location, cyclingTypes, availability, MinDistance,
                    MaxDistance, Speed)
                {
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt
                };
            }
        }

        public async Task<Profile?> GetByUserId(Guid userId)
        {
            await using var connection = _connectionFactory.Create();
            var result = await connection.QueryFirstOrDefaultAsync<ProfileQueryResult>(
                @"SELECT 
                    user_id,
                    display_name,
                    location_name,
                    min_distance,
                    max_distance,
                    speed,
                    cycling_types,
                    availability,
                    ST_X(location::geometry) as longitude,
                    ST_Y(location::geometry) as latitude,
                    created_at,
                    updated_at
                    FROM user_profile 
                    WHERE user_id=@UserId",
                new
                {
                    UserId = userId
                }
            );

            return result?.ToProfile();
        }

        public async Task<bool> UpsertProfile(Profile profile)
        {
            var profileExists = await HasProfile(profile.UserId);

            await using var connection = _connectionFactory.Create();
            if (profileExists)
            {
                return await connection.ExecuteAsync(
                    @"UPDATE user_profile
                        SET display_name=@DisplayName,
                        location_name=@LocationName,
                        min_distance=@MinDistance,
                        max_distance=@MaxDistance,
                        speed=@Speed,
                        cycling_types=@CyclingTypes,
                        availability=@Availability,
                        location=ST_POINT(@Longitude, @Latitude)
                        WHERE user_id=@UserId",
                    new
                    {
                        profile.UserId,
                        profile.DisplayName,
                        profile.LocationName,
                        profile.MinDistance,
                        profile.MaxDistance,
                        profile.Speed,
                        CyclingTypes = profile.CyclingTypes.Select(f => f.ToString()).ToImmutableList(),
                        Availability= profile.Availability.Select(f => f.ToString()).ToImmutableList(),
                        Longitude=profile.Location.Longitude,
                        Latitude=profile.Location.Latitude
                    }
                ) == 1;
            }
            
            return await connection.ExecuteAsync(
                @"INSERT INTO user_profile (user_id, display_name, location_name, min_distance, max_distance, speed, cycling_types, availability, location)
                        VALUES(@UserId, @DisplayName, @LocationName, @MinDistance, @MaxDistance, @Speed, @CyclingTypes, @Availability, ST_POINT(@Longitude, @Latitude))",
                new
                {
                    profile.UserId,
                    profile.DisplayName,
                    profile.LocationName,
                    profile.MinDistance,
                    profile.MaxDistance,
                    profile.Speed,
                    CyclingTypes = profile.CyclingTypes.Select(f => f.ToString()).ToImmutableList(),
                    Availability= profile.Availability.Select(f => f.ToString()).ToImmutableList(),
                    Longitude=profile.Location.Longitude,
                    Latitude=profile.Location.Latitude
                }
            ) == 1;
        }
    }
}