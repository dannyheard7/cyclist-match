using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence;
using RuntimeService.DTO;

namespace RuntimeService.Services
{
    public class ProfileMatchService : IProfileMatchService
    {
        public async Task<IEnumerable<ProfileMatch>> GetProfileMatches(IUser user, int maxResults=15, int? page=null)
        {
            return new List<ProfileMatch>();
        }
    }
}