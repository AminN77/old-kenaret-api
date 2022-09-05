using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface INodeRepository
    {
        void CreateNode(Node node);
        Task CreateNodeAsync(Node node);
        Task<IEnumerable<Node>> GetAllNodesForLiveStreamAsync(Guid liveStreamId, bool trackChanges);
    }
}