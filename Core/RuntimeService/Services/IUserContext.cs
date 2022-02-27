using System.Threading.Tasks;
using Persistence.Profile.Types.DTO;
using RuntimeService.Controllers.Models;

namespace RuntimeService.Services;

public interface IUserContext
{
    public string BearerToken { get; }
    
    public Task<ProfileDTO> GetUserProfile();
    
    public Task<ProfileDTO?> GetUserProfileOrDefault();

    public Task<ProfileDTO> CreateProfileForUser(ProfileInput input);
}