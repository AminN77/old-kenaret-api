using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class NodeRepository :RepositoryBase<Node> , INodeRepository 
    {
        public NodeRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateNode(Node node)
        {
            Create(node);
        }

        public async Task CreateNodeAsync(Node node)
        {
            await CreateAsync(node);
        }
        
        public async Task<IEnumerable<Node>> GetAllNodesForLiveStreamAsync(Guid liveStreamId,
            bool trackChanges)
        {
            return await FindByCondition(n => n.LiveStreamId.Equals(liveStreamId), trackChanges: trackChanges)
                .ToListAsync();
        }
    }
}