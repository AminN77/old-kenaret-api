using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateUser(User user) => Create(user);


        public async Task CreateUserAsync(User user)
        {
            await CreateAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges)
            => await FindAll(trackChanges)
                .OrderBy(u => u.LastName)
                .ToListAsync();


        public async Task<User> GetUserByIdAsync(Guid userId, bool trackChanges)
            =>
                await FindByCondition(c => c.Id.Equals(userId), trackChanges)
                    .SingleOrDefaultAsync();

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber, bool trackChanges)
            =>
                await FindByCondition(c => c.PhoneNumber.Equals(phoneNumber), trackChanges)
                    .SingleOrDefaultAsync();

        public async Task<User> GetUserByUserNameAsync(string userName, bool trackChanges)
            =>
                await FindByCondition(c => c.Username.Equals(userName), trackChanges)
                    .SingleOrDefaultAsync();
    }
}