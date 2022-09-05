using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface ILinkRepository
    {
        void CreateLink(Link link);
        Task CreateLinkAsync(Link link);
        Task<IEnumerable<Link>> GetAllLinksAsync(bool trackChanges);
        Task<Link> GetLinkAsync(Guid linkId, bool trackChanges);
        void DeleteLink(Link link);
    }
}