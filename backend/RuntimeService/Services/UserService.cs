using System;
using System.Threading.Tasks;
using Persistence;
using Persistence.Repository;

namespace RuntimeService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageService _messageService;

        public UserService(IUserRepository userRepository, IMessageService messageService)
        {
            _userRepository = userRepository;
            _messageService = messageService;
        }

        public Task<IUser?> GetUserById(Guid userId)
        {
            return _userRepository.GetUserDetailsByInternalId(userId);
        }

        public async Task DeleteUser(IUser user)
        {
            await _messageService.DeleteUserConversations(user);
            await _userRepository.DeleteUser(user);
        }
    }
}