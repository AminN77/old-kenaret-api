using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.RequestFeatures;

namespace Contracts.Repository
{
    public interface ILiveStreamRepository
    {
        void CreateLiveStream(LiveStream liveStream);
        Task CreateLiveStreamAsync(LiveStream liveStream);
        
        Task<PagedList<LiveStream>> GetAllLiveStreamsAsync(LiveStreamParameters liveStreamParameters, bool trackChanges);
        Task<List<LiveStream>> GetAllLiveStreamsAsync(bool trackChanges);
        Task<LiveStream> GetLiveStreamAsync(Guid liveStreamId, bool trackChanges);
        
    }
}