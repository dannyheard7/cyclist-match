using System;
using System.Threading.Tasks;
using Auth;
using Persistence.Entity;
using Persistence.Types.DTO;
using RuntimeService.ProfileApi;

namespace RuntimeService.Services
{
    public interface IProfileService
    {
        public Task<ProfileDTO?> GetByOidcUser(IOIDCUser oidcUser);
        public Task<ProfileDTO?> GetById(Guid userId);
        public Task Create(CreateProfileDTO profileInput);
    }
}