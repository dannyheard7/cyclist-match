using System;
using System.Threading.Tasks;
using Persistence;
using Persistence.Repository;

namespace RuntimeService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IUser?> GetUserById(Guid userId)
        {
            return _userRepository.GetUserDetailsByInternalId(userId);
        }
    }
}