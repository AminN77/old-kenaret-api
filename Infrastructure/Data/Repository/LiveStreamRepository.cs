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
    public class LiveStreamRepository : RepositoryBase<LiveStream>, ILiveStreamRepository
    {
        public LiveStreamRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateLiveStream(LiveStream liveStream)
        {
            Create(liveStream);
        }

        public async Task CreateLiveStreamAsync(LiveStream liveStream)
        {
            await CreateAsync(liveStream);
        }

        public async Task<PagedList<LiveStream>> GetAllLiveStreamsAsync(LiveStreamParameters liveStreamParameters,
            bool trackChanges)
        {
            var liveStreams = await FindAll(trackChanges)
                .OrderByDescending(c => c.CreateDateTime)
                .Where(l => !l.IsFinished)
                .ToListAsync();

            return PagedList<LiveStream>
                .ToPagedList(liveStreams, liveStreamParameters.PageNumber, liveStreamParameters.PageSize);
        }

        public async Task<List<LiveStream>> GetAllLiveStreamsAsync(bool trackChanges)
        {
            var liveStreams = await FindAll(trackChanges)
                .Where(l => !l.IsFinished)
                .ToListAsync();

            return liveStreams;
        }

        public async Task<LiveStream> GetLiveStreamAsync(Guid liveStreamId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(liveStreamId), trackChanges)
                .SingleOrDefaultAsync();
    }
}