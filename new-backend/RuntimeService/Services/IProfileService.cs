using System;
using System.Threading.Tasks;
using Persistence.Entity;

namespace RuntimeService.Services
{
    public interface IProfileService
    {
        public Task<Profile?> Get(Guid userId);
    }
}