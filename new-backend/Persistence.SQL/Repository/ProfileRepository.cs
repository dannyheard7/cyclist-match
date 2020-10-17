using System;
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

        public async Task<Profile?> GetByUserId(Guid userId)
        {
            await using var connection = _connectionFactory.Create();
            return await connection.QueryFirstOrDefaultAsync<Profile>(
                "SELECT * FROM user_profile WHERE user_id=@UserId",
                new
                {
                    UserId = userId
                }
            );
        }
    }
}