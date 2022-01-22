using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Persistence.SQL
{
    internal class ConnectionFactory 
    {
        private readonly string _connectonString;

        public ConnectionFactory(string connectionString)
        {
            _connectonString = connectionString;
        }

        public NpgsqlConnection Create() =>
            new NpgsqlConnection(_connectonString);
    }
}