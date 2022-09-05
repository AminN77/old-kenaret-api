using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Link;
using BusinessLogic.DataTransferObjects.LiveStream;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.Repository;
using Domain.Entities;
using Domain.Events.LiveStreamEvents;
using Domain.Events.ParticipantsEvents;
using Domain.RequestFeatures;

namespace BusinessLogic.Implementation
{
    public class LiveStreamBusinessLogic : ILiveStreamBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public event ILiveStreamBusinessLogic.LiveStreamEventHandler LiveStreamEventOccured;

        public LiveStreamBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper,
            ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        #region Methods

        #region publics

        public async Task<IBusinessLogicResult<LiveStreamForShowDto>> CreateLiveStreamAsync(
            LiveStreamForCreationDto liveStreamForCreationDto,
            string streamerUsername, Guid requestId)
        {
            //validate
            var tempLive = await _mapper.MapAsync(liveStreamForCreationDto, new LiveStreamForCreationDto());
            tempLive.Links = null;
            var live = await _mapper.MapAsync(tempLive, new LiveStream());
            var user = await _repositoryManager.User.GetUserByUserNameAsync(streamerUsername, trackChanges: false);
            live.Id = Guid.NewGuid();
            live.CreateDateTime = DateTime.UtcNow;
            live.StreamerId = user.Id;
            await _repositoryManager.LiveStream.CreateLiveStreamAsync(live);
            await _repositoryManager.SaveAsync();
            var linksIdList = new List<Guid>();
            var linksForShow = new List<LinkForShowDto>();
            foreach (var link in liveStreamForCreationDto.Links)
            {
                var linkEntity = await _mapper.MapAsync(link, new Link());
                linkEntity.Id = Guid.NewGuid();
                await _repositoryManager.Link.CreateLinkAsync(linkEntity);
                linksIdList.Add(linkEntity.Id);
                linksForShow.Add(await _mapper.MapAsync(linkEntity, new LinkForShowDto()));
            }

            await AddNodesAsync(new LiveStreamLinksForUpdateDto {Links = linksIdList.ToArray()}, live.Id, requestId);
            await _repositoryManager.SaveAsync();
            var liveForShow = await _mapper.MapAsync(live, new LiveStreamForShowDto());
            liveForShow.Nodes = linksForShow.ToArray();
            liveForShow.StreamerUsername = user.Username;
            liveForShow.StreamerAvatarAddress = user.Avatar;
            liveForShow.StreamerFullName = user.FirstName + " " + user.LastName;
            var tempWorld =
                await _repositoryManager.World.GetWorldAsync(liveStreamForCreationDto.WorldId, trackChanges: false);
            liveForShow.WorldTitle = tempWorld.Title;
            return new BusinessLogicResult<LiveStreamForShowDto> {Success = true, Result = liveForShow};
        }
        
        public async Task<IBusinessLogicResult> AddNodesAsync(
            LiveStreamLinksForUpdateDto liveStreamLinksForUpdateDto, Guid liveStreamId, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: true);
            foreach (var link in liveStreamLinksForUpdateDto.Links)
            {
                await _repositoryManager.Nodes.CreateNodeAsync(new Node() {LinkId = link, LiveStreamId = liveStreamId});
                await _repositoryManager.SaveAsync();
            }

            return new BusinessLogicResult() {Success = true};
        }


        public async Task<IBusinessLogicResult<PagedList<LiveStreamForShowDto>>> GetAllLiveStreamAsync(
            LiveStreamParameters liveStreamParameters, Guid requestId)
        {
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

        public async Task<IBusinessLogicResult<LiveStreamForShowDto>> GetLiveStreamByIdAsync(Guid liveStreamId, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: false);
            if (live is null)
            {
                return new BusinessLogicResult<LiveStreamForShowDto>
                    {Success = false, Error = BusinessLogicErrors.LiveStream.LiveStreamDoesNotExist};
            }

            var tempLive = await _mapper.MapAsync(live, new LiveStream());
            var liveForTransfer = await _mapper.MapAsync(tempLive, new LiveStreamForShowDto());
            var tempWorld = await _repositoryManager.World.GetWorldAsync(live.WorldId, trackChanges: false);
            liveForTransfer.WorldTitle = tempWorld.Title;
            var tempNodes = await GetNodesAsync(liveForTransfer.Id);
            liveForTransfer.Nodes = tempNodes.Result.ToArray();

            var user = await _repositoryManager.User.GetUserByIdAsync(live.StreamerId, trackChanges: false);
            liveForTransfer.StreamerUsername = user.Username;
            liveForTransfer.StreamerAvatarAddress = user.Avatar;
            liveForTransfer.StreamerFullName = user.FirstName + " " + user.LastName;
            return new BusinessLogicResult<LiveStreamForShowDto> {Success = true, Result = liveForTransfer};
        }

        public async Task<IBusinessLogicResult> UpdateLiveStreamAsync(Guid liveStreamId,
            LiveStreamForUpdateDto liveStreamForUpdateDto, Guid requestId)
        {
            var liveStreamUpdatedEvent = new LiveStreamUpdatedEventArgs();
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: true);
            if (live is null)
                return new BusinessLogicResult
                    {Success = false, Error = BusinessLogicErrors.LiveStream.LiveStreamDoesNotExist};

            liveStreamUpdatedEvent.BeforeChange = live;
            await _mapper.MapAsync(liveStreamForUpdateDto, live);
            await _repositoryManager.SaveAsync();
            liveStreamUpdatedEvent.AfterChange = live;
            OnDomainEventOccured(liveStreamUpdatedEvent, requestId);

            return new BusinessLogicResult {Success = true};
        }

        public async Task<IBusinessLogicResult> UpdateLiveStreamParticipantsAsync(Guid liveStreamId,
            LiveStreamParticipantForUpdateDto liveStreamParticipantForUpdateDto, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: true);
            if (live is null)
                return new BusinessLogicResult()
                    {Success = false, Error = BusinessLogicErrors.LiveStream.LiveStreamDoesNotExist};

            var participantsForLiveStream = await _repositoryManager.Participants.GetAllParticipantsForLiveStreamAsync(
                liveStreamId,
                trackChanges: true);

            var participant = participantsForLiveStream
                .SingleOrDefault(p => p.UserId.Equals(liveStreamParticipantForUpdateDto.UserId));

            if (participant is null)
            {
                var newParticipant = new Participants()
                {
                    IsConnected = liveStreamParticipantForUpdateDto.IsConnected,
                    UserId = liveStreamParticipantForUpdateDto.UserId,
                    LiveStreamId = liveStreamId,
                    LastStatusChangeDateTime = DateTime.UtcNow
                };

                await _repositoryManager.Participants.CreateParticipantAsync(newParticipant);
            }
            else
            {
                participant.IsConnected = liveStreamParticipantForUpdateDto.IsConnected;
                var time = DateTime.UtcNow;
                participant.LastStatusChangeDateTime = time;
                if (!participant.IsConnected)
                {
                    var newEvent = new ParticipantStatusChangedEventArgs
                    {
                        Participant = participant,
                        JoinDateTime = participant.LastStatusChangeDateTime,
                        LeaveDateTime = time,
                        Duration = time - participant.LastStatusChangeDateTime
                    };

                    participant.TotalDuration += newEvent.Duration;
                    OnDomainEventOccured(newEvent, requestId);
                }
            }

            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult() {Success = true};
        }

        public async Task<IBusinessLogicResult<IEnumerable<LiveStreamParticipantsForShowDto>>>
            GetAllLiveStreamParticipantsAsync(Guid liveStreamId, Guid requestId)
        {
            var participantsId =
                await _repositoryManager.Participants.GetAllParticipantsForLiveStreamAsync(liveStreamId,
                    trackChanges: false);

            var participantsForShow = new List<LiveStreamParticipantsForShowDto>();
            foreach (var par in participantsId)
            {
                var user = await _repositoryManager.User.GetUserByIdAsync(par.UserId, trackChanges: false);
                participantsForShow.Add(await _mapper.MapAsync(user, new LiveStreamParticipantsForShowDto()));
            }

            return new BusinessLogicResult<IEnumerable<LiveStreamParticipantsForShowDto>>()
                {Success = true, Result = participantsForShow};
        }

        public async Task<IBusinessLogicResult<IEnumerable<LiveStreamLinksForShowDto>>> GetAllLiveStreamNodesAsync(
            Guid liveStreamId, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: false);
            var nodes = await _repositoryManager.Nodes.GetAllNodesForLiveStreamAsync(liveStreamId, trackChanges: false);
            var linksForShow = new List<LiveStreamLinksForShowDto>();
            foreach (var node in nodes)
            {
                var link = await _repositoryManager.Link.GetLinkAsync(node.LinkId, trackChanges: false);
                linksForShow.Add(await _mapper.MapAsync(link, new LiveStreamLinksForShowDto()));
            }

            return new BusinessLogicResult<IEnumerable<LiveStreamLinksForShowDto>>()
                {Success = true, Result = linksForShow};
        }


        public async Task<IBusinessLogicResult> FinishLiveStreamAsync(Guid liveStreamId, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: true);

            if (live is null)
                return new BusinessLogicResult()
                    {Success = false, Error = BusinessLogicErrors.LiveStream.LiveStreamDoesNotExist};

            live.EndDateTime = DateTime.UtcNow;
            live.IsFinished = true;
            await _repositoryManager.SaveAsync();
            await DisconnectAllParticipantsFromLiveStream(liveStreamId, requestId);
            return new BusinessLogicResult() {Success = true};
        }

        public async Task<IBusinessLogicResult> FinishAllLiveStreamAsync(Guid requestId)
        {
            var lives = await _repositoryManager.LiveStream.GetAllLiveStreamsAsync(trackChanges: true);
            foreach (var live in lives)
            {
                live.IsFinished = true;
                await _repositoryManager.SaveAsync();
                await DisconnectAllParticipantsFromLiveStream(live.Id, requestId);
            }

            return new BusinessLogicResult() {Success = true};
        }

        public async Task<IBusinessLogicResult> AddGuestToLiveStreamAsync(Guid liveStreamId, Guid requestId)
        {
            var live = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: true);
            if (live is null)
                return new BusinessLogicResult()
                    {Success = false, Error = BusinessLogicErrors.LiveStream.LiveStreamDoesNotExist};

            live.GuestsCount++;
            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult() {Success = true};
        }


        public async Task<IBusinessLogicResult<LiveStreamParticipantsForShowDto>> GetLiveStreamParticipantAsync
            (Guid liveStreamId, Guid participantId, Guid requestId)
        {
            var participantsId = await _repositoryManager.Participants.GetAllParticipantsForLiveStreamAsync(liveStreamId, trackChanges: false);
            var exactParticipant = participantsId.SingleOrDefault(x => x.UserId == participantId);
            if (exactParticipant == null)
                return new BusinessLogicResult<LiveStreamParticipantsForShowDto>()
                { Success = false, Error = BusinessLogicErrors.LiveStream.ParticipantDoesNotExist };

            var user = await _repositoryManager.User.GetUserByIdAsync(exactParticipant.UserId, trackChanges: false);
            var dto = new LiveStreamParticipantsForShowDto { Avatar = user.Avatar, FirstName = user.FirstName };
            return new BusinessLogicResult<LiveStreamParticipantsForShowDto>() { Error = null, Result = dto, Success = true };
        }

        #endregion

        #region privates

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

        private async Task<IBusinessLogicResult<LiveStream>> AddNodesAsync(
            LiveStreamLinksForUpdateDto liveStreamLinksForUpdateDto, LiveStream liveStream)
        {
            foreach (var link in liveStreamLinksForUpdateDto.Links)
            {
                await _repositoryManager.Nodes.CreateNodeAsync(new Node()
                    {LinkId = link, LiveStreamId = liveStream.Id});
                await _repositoryManager.SaveAsync();
            }

            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult<LiveStream>() {Success = true, Result = liveStream};
        }

        private async Task DisconnectAllParticipantsFromLiveStream(Guid liveStreamId, Guid requestId)
        {
            var liveStream = await _repositoryManager.LiveStream.GetLiveStreamAsync(liveStreamId, trackChanges: false);
            if (liveStream is null)
            {
                throw new InvalidOperationException(
                    $"No related livestream exist to disconnect all participants. id : {liveStreamId}");
            }

            var participantsForLiveStream = await _repositoryManager.Participants.GetAllParticipantsForLiveStreamAsync(
                liveStreamId,
                trackChanges: true);

            foreach (var participant in participantsForLiveStream)
            {
                if (!participant.IsConnected) continue;
                var time = DateTime.UtcNow;
                var newEvent = new ParticipantStatusChangedEventArgs
                {
                    Participant = participant,
                    JoinDateTime = participant.LastStatusChangeDateTime,
                    LeaveDateTime = time,
                    Duration = time - participant.LastStatusChangeDateTime
                };
                    
                participant.IsConnected = false;
                participant.LastStatusChangeDateTime = time;
                participant.TotalDuration += newEvent.Duration;
                OnDomainEventOccured(newEvent, requestId);
                await _repositoryManager.SaveAsync();
            }
        }

        #endregion

        #region protecteds

        protected virtual void OnDomainEventOccured(EventArgs e, Guid requestId)
        {
            LiveStreamEventOccured?.Invoke(this, e, requestId);
        }


        #endregion

        #endregion
    }
}