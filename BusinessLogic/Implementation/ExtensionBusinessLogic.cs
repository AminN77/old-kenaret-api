using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Extension;
using BusinessLogic.DataTransferObjects.Feedback;
using BusinessLogic.DataTransferObjects.Link;
using BusinessLogic.DataTransferObjects.LiveStream;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.Repository;
using Domain.Entities;
using Domain.RequestFeatures;

namespace BusinessLogic.Implementation
{
    public class ExtensionBusinessLogic : IExtensionBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public ExtensionBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<IBusinessLogicResult<PagedList<LiveStreamForShowDto>>> StreamDetectionAsync(StreamDetectionDto detectionDto, Guid requestId)
        {
            var liveStreamParameters = new LiveStreamParameters{ PageNumber =1, PageSize= 100};
            var lives = await _repositoryManager.LiveStream.GetAllLiveStreamsAsync(liveStreamParameters,
                trackChanges: false);
            var livesListForTransfer = new List<LiveStreamForShowDto>();
            foreach (var live in lives)
            {
                var liveDto = await _mapper.MapAsync(live, new LiveStreamForShowDto());
                var user = await _repositoryManager.User.GetUserByIdAsync(live.StreamerId, trackChanges: false);
                liveDto.StreamerUsername = user.Username;
                var tempNodes = await GetNodesAsync(live.Id);
                liveDto.Nodes = tempNodes.Result.ToArray();
                liveDto.StreamerAvatarAddress = user.Avatar;
                liveDto.StreamerFullName = user.FirstName + " " + user.LastName;
                var tempWorld = await _repositoryManager.World.GetWorldAsync(live.WorldId, trackChanges: false);
                liveDto.WorldTitle = tempWorld.Title;
                livesListForTransfer.Add(liveDto);
            }

            var livesListForTransferPaged = new PagedList<LiveStreamForShowDto>(livesListForTransfer,
                lives.MetaData.TotalCount, lives.MetaData.CurrentPage, lives.MetaData.PageSize);
            return new BusinessLogicResult<PagedList<LiveStreamForShowDto>>
                {Success = true, Result = livesListForTransferPaged};

        }

        private async Task<IBusinessLogicResult<IEnumerable<LinkForShowDto>>> GetNodesAsync(Guid liveStreamId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: false);
            var nodes = await _repositoryManager.Nodes.GetAllNodesForLiveStreamAsync(liveStreamId, trackChanges: false);
            var linksForShow = new List<LinkForShowDto>();
            foreach (var node in nodes)
            {
                var link = await _repositoryManager.Link.GetLinkAsync(node.LinkId, trackChanges: false);
                linksForShow.Add(await _mapper.MapAsync(link, new LinkForShowDto()));
            }

            return new BusinessLogicResult<IEnumerable<LinkForShowDto>>()
                {Success = true, Result = linksForShow};
        }
    }
}