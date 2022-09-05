using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.LiveStream;
using Contracts.LoggerManager;
using Domain.RequestFeatures;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers
{
    [ApiController]
    [Route("api/livestreams/")]
    public class LiveStreamController : ControllerBase
    {
        private readonly ILiveStreamBusinessLogic _liveStreamBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;


        public LiveStreamController(ILiveStreamBusinessLogic liveStreamBusinessLogic, ILoggerManager loggerManager, EventHandlersManager eventHandlersManager)
        {
            _liveStreamBusinessLogic = liveStreamBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = eventHandlersManager;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateNewLiveStream(
            [FromBody] LiveStreamForCreationDto liveStreamForCreationDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(liveStreamForCreationDto, requestId);
            var streamerUsername = HttpContext.User.Identity.Name;
            var res = await _liveStreamBusinessLogic.CreateLiveStreamAsync(liveStreamForCreationDto, streamerUsername, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPost("{id}/links")]
        public async Task<IActionResult> AddLinksToLiveStream([FromRoute] Guid id,
            LiveStreamLinksForUpdateDto liveStreamLinksForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(new { liveStreamLinksForUpdateDto, id }, requestId);
            var res = await _liveStreamBusinessLogic.AddNodesAsync(liveStreamLinksForUpdateDto, id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{id}/links")]
        public async Task<IActionResult> GetAllLinksForLiveStream([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _liveStreamBusinessLogic.GetAllLiveStreamNodesAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLiveStreams([FromQuery] LiveStreamParameters liveStreamParameters)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(liveStreamParameters, requestId);
            var res = await _liveStreamBusinessLogic.GetAllLiveStreamAsync(liveStreamParameters, requestId);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(res.Result.MetaData));
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLiveStreamById([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _liveStreamBusinessLogic.GetLiveStreamByIdAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateLiveStream([FromRoute] Guid id,
            [FromBody] LiveStreamForUpdateDto liveStreamForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(new { liveStreamForUpdateDto, id }, requestId);
            var res = await _liveStreamBusinessLogic.UpdateLiveStreamAsync(id, liveStreamForUpdateDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut("{id}/participants"), Authorize]
        public async Task<IActionResult> UpdateLiveStreamParticipant([FromRoute] Guid id,
            [FromBody] LiveStreamParticipantForUpdateDto liveStreamParticipantForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(new { liveStreamParticipantForUpdateDto, id }, requestId);
            var res = await _liveStreamBusinessLogic.UpdateLiveStreamParticipantsAsync(id,
                liveStreamParticipantForUpdateDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{id}/participants")]
        public async Task<IActionResult> GetAllLiveStreamParticipants([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _liveStreamBusinessLogic.GetAllLiveStreamParticipantsAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{liveid}/participants/{participantid}")]
        public async Task<IActionResult> GetLiveStreamParticipant([FromRoute] Guid liveid, [FromRoute] Guid participantid)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(new { liveid, participantid }, requestId);
            var res = await _liveStreamBusinessLogic.GetLiveStreamParticipantAsync(liveid, participantid, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut("{id}/finish"), Authorize]
        public async Task<IActionResult> FinishLiveStream([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _liveStreamBusinessLogic.FinishLiveStreamAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut("{id}/addguest")]
        public async Task<IActionResult> AddGuestToLiveStream([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _liveStreamBusinessLogic.AddGuestToLiveStreamAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        #region privates

        private void RequestLogger(object dto, Guid requestId)
        {
            HttpContext.Request.Headers.Add("X-request-id", requestId.ToString());
            var request = HttpContext.Request;
            var info = new
            {
                Method = request.Method,
                Header = request.Headers,
                QueryString = request.QueryString
            };

            _loggerManager.LogDebug("Request: " + JsonConvert.SerializeObject(info), requestId);
            if (dto is null) return;
            _loggerManager.LogDebug("Request Body: " + JsonConvert.SerializeObject(dto), requestId);
        }

        #endregion

    }
}