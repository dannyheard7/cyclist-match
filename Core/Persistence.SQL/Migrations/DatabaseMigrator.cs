using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Persistence.SQL.Migrations
{
    internal class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly ILogger<DatabaseMigrator> _logger;
        
        private ConnectionFactory _connectionFactory;

        public DatabaseMigrator(
            ConnectionFactory connectionFactory,
            ILogger<DatabaseMigrator> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task Migrate()
        {
            await using var connection = _connectionFactory.Create();

            var evolve = new Evolve.Evolve(connection, msg => _logger.LogInformation(msg))
            {
                EmbeddedResourceAssemblies = new[] { typeof(DatabaseMigrator).Assembly },
                EnableClusterMode = true,
                IsEraseDisabled = true,
                MetadataTableName = "migrations_changelog",
                OutOfOrder = true
            };

            evolve.Migrate();
        }
    }
}