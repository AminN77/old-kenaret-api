using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class WorldRepository : RepositoryBase<World>, IWorldRepository
    {
        public WorldRepository(PgSqlDbContext repositoryContext) 
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<World>> GetAllWorldsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.Title)
                .ToListAsync();

        public async Task<World> GetWorldAsync(Guid worldId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(worldId), trackChanges)
                .SingleOrDefaultAsync();

        public void CreateWorld(World world) => Create(world);

        public async Task<IEnumerable<World>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToListAsync();

        public void DeleteWorld(World world)
        {
            Delete(world);
        }
    }
}