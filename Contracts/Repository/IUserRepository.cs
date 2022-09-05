using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        Task CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
        Task<User> GetUserByIdAsync(Guid userId, bool trackChanges);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber, bool trackChanges);
        Task<User> GetUserByUserNameAsync(string userName, bool trackChanges);
    }
}