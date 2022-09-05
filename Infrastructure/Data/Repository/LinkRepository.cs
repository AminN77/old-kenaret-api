using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Domain.RequestFeatures;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        public LinkRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateLink(Link link)
        {
            Create(link);
        }

        public async Task CreateLinkAsync(Link link)
        {
            await CreateAsync(link);
        }

        public async Task<IEnumerable<Link>> GetAllLinksAsync(bool trackChanges)
            => await FindAll(trackChanges)
                .OrderBy(l => l.Description)
                .ToListAsync();


        public async Task<Link> GetLinkAsync(Guid linkId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(linkId), trackChanges)
                .SingleOrDefaultAsync();


        public void DeleteLink(Link link)
        {
            Delete(link);
        }
    }
}