using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Persistence.SQL
{
    internal class ConnectionFactory 
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public NpgsqlConnection Create() =>
            new NpgsqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
    }
}