using System;
using System.Threading.Tasks;
using Dapper;
using Persistence.Repository;
using Persistence.SQL.Objects;

namespace Persistence.SQL.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public UserRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<bool> ExternalUserHasProfile(string externalUserId)
        {
            await using var connection = _connectionFactory.Create();
            return await connection.QueryFirstOrDefaultAsync<bool>(
                @"SELECT EXISTS(SELECT 1 FROM user_profile INNER JOIN ""user"" u ON u.id=user_profile.user_id WHERE u.external_id=@ExternalUserId)",
                new
                {
                    ExternalUserId = externalUserId
                }
            );
        }

        public async Task<IUser> GetUserDetails(string externalUserId)
        {
            await using var connection = _connectionFactory.Create();
            return await connection.QueryFirstAsync<DBUser>(
                @"SELECT * FROM ""user"" WHERE external_id=@ExternalUserId",
                new
                {
                    ExternalUserId = externalUserId
                }
            );
        }

        public async Task<bool> UpdateUserDetails(IUser user)
        {
            await using var connection = _connectionFactory.Create();
            var result = await connection.ExecuteAsync(
                @"INSERT INTO ""user"" (external_id, given_names, family_name, email, picture)
                    VALUES(@ExternalId, @GivenNames, @FamilyName, @Email, @Picture) 
                    ON CONFLICT (external_id) 
                    DO 
                    UPDATE SET given_names = @GivenNames,
                    family_name=@FamilyName,
                    email=@Email,
                    picture=@Picture",
                new
                {
                    user.ExternalId,
                    user.GivenNames,
                    user.FamilyName,
                    user.Email,
                    user.Picture
                }
            );
            return result == 1;
        }
    }
}