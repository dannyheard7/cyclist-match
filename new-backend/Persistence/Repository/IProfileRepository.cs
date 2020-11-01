using System;
using System.Threading.Tasks;
using Persistence.Entity;

namespace Persistence.Repository
{
    public interface IProfileRepository
    {
        public Task<Profile?> GetByUserId(Guid userId);

        public Task<bool> UpsertProfile(Profile profile);
    }
}