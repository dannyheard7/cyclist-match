using System.Threading.Tasks;

namespace Persistence.SQL.Migrations
{
    public interface IDatabaseMigrator
    {
        public Task Migrate();
    }
}