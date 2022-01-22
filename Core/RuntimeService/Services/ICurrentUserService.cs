using System.Threading.Tasks;

namespace Auth
{
    public interface ICurrentUserService
    {
        public Task<IOIDCUser> GetUser();
    }
}