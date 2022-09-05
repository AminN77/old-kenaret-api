using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.LiveStream;
using Contracts.LoggerManager;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers.Admin
{
    [ApiController]
    [ServiceFilter(typeof(AdminActionFilter))]
    [Route("api/admin/livestreams")]
    public class LiveStreamController : ControllerBase
    {
        private readonly ILiveStreamBusinessLogic _liveStreamBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;

        public LiveStreamController(ILiveStreamBusinessLogic liveStreamBusinessLogic, ILoggerManager loggerManager, EventHandlersManager elasticEventHandler)
        {
            _liveStreamBusinessLogic = liveStreamBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = elasticEventHandler;
        }

        [HttpPut("{id}/participants")]
        public async Task<IActionResult> UpdateLiveStreamParticipant([FromRoute] Guid id,
            [FromBody] LiveStreamParticipantForUpdateDto liveStreamParticipantForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(liveStreamParticipantForUpdateDto, requestId);
            var res = await _liveStreamBusinessLogic.UpdateLiveStreamParticipantsAsync(id,
                liveStreamParticipantForUpdateDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok() : StatusCode(500, res.Error);
        }

        [HttpPut("{id}/finish")]
        public async Task<IActionResult> FinishLiveStream([FromRoute] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(null, requestId);
            var res = await _liveStreamBusinessLogic.FinishLiveStreamAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok() : StatusCode(500, res.Error);
        }

        [HttpPut("finishall")]
        public async Task<IActionResult> FinishAllLiveStreams()
        {
            var requestId = Guid.NewGuid();
            RequestLogger(null, requestId);
            var res = await _liveStreamBusinessLogic.FinishAllLiveStreamAsync(requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok() : StatusCode(500, res.Error);
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