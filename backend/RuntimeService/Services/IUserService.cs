using System;
using System.Threading.Tasks;
using Persistence;

namespace RuntimeService.Services
{
    public interface IUserService
    {
        public Task<IUser?> GetUserById(Guid userId);
    }
}