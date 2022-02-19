using System.Threading.Tasks;

namespace Auth;

public interface IUserContext
{
    public string BearerToken { get; }
    public Task<IOIDCUser> GetUser();
}