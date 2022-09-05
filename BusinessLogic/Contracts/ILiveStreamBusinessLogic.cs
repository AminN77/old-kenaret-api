using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.LiveStream;
using Domain.RequestFeatures;

namespace BusinessLogic.Contracts
{
    public interface ILiveStreamBusinessLogic
    {
        public delegate void LiveStreamEventHandler(object sender, EventArgs args, Guid requestId);
        public event LiveStreamEventHandler LiveStreamEventOccured;
        Task<IBusinessLogicResult<LiveStreamForShowDto>> CreateLiveStreamAsync(LiveStreamForCreationDto liveStreamForCreationDto, string streamerUsername, Guid requestId);
        Task<IBusinessLogicResult> AddNodesAsync(LiveStreamLinksForUpdateDto liveStreamLinksForUpdateDto, Guid liveStreamId, Guid requestId);
        Task<IBusinessLogicResult<PagedList<LiveStreamForShowDto>>> GetAllLiveStreamAsync(LiveStreamParameters liveStreamParameters, Guid requestId);
        Task<IBusinessLogicResult<LiveStreamForShowDto>> GetLiveStreamByIdAsync(Guid liveStreamId, Guid requestId);
        Task<IBusinessLogicResult> UpdateLiveStreamAsync(Guid liveStreamId, LiveStreamForUpdateDto liveStreamForUpdateDto, Guid requestId);
        Task<IBusinessLogicResult> UpdateLiveStreamParticipantsAsync(Guid liveStreamId, LiveStreamParticipantForUpdateDto liveStreamParticipantForUpdateDto, Guid requestId);
        Task<IBusinessLogicResult<IEnumerable<LiveStreamParticipantsForShowDto>>> GetAllLiveStreamParticipantsAsync(Guid liveStreamId, Guid requestId);
        Task<IBusinessLogicResult<LiveStreamParticipantsForShowDto>> GetLiveStreamParticipantAsync(Guid liveStreamId, Guid participantId, Guid requestId);
        Task<IBusinessLogicResult<IEnumerable<LiveStreamLinksForShowDto>>> GetAllLiveStreamNodesAsync(Guid liveStreamId, Guid requestId);
        Task<IBusinessLogicResult> FinishLiveStreamAsync(Guid liveStreamId, Guid requestId);
        Task<IBusinessLogicResult> FinishAllLiveStreamAsync(Guid requestId);
        Task<IBusinessLogicResult> AddGuestToLiveStreamAsync(Guid liveStreamId, Guid requestId);
    }
}