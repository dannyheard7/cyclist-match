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
        }

        public NpgsqlConnection Create() =>
            new NpgsqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
    }
}