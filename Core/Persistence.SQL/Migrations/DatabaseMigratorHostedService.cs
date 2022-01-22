using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Persistence.SQL.Migrations
{
    internal class DatabaseMigratorHostedService : IHostedService
    {
        private readonly IDatabaseMigrator _databaseMigrator;

        public DatabaseMigratorHostedService(IDatabaseMigrator databaseMigrator)
        {
            _databaseMigrator = databaseMigrator;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _databaseMigrator.Migrate();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}