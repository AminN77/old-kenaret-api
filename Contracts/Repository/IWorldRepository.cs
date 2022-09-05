using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface IWorldRepository
    {
        Task<IEnumerable<World>> GetAllWorldsAsync(bool trackChanges);
        Task<World> GetWorldAsync(Guid worldId, bool trackChanges);
        void CreateWorld(World world);
        Task<IEnumerable<World>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteWorld(World world);
    }
}