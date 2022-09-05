using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Link;
using Contracts.LoggerManager;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers
{
    [ApiController]
    [Route("api/links")]

    public class LinkController : ControllerBase
    {
        private readonly ILinkBusinessLogic _linkBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;

        public LinkController(ILinkBusinessLogic linkBusinessLogic, ILoggerManager loggerManager, EventHandlersManager eventHandlersManager)
        {
            _linkBusinessLogic = linkBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = eventHandlersManager;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateNewLink([FromBody] LinkForCreationDto linkForCreationDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(linkForCreationDto, requestId);
            var res = await _linkBusinessLogic.CreateLinkAsync(linkForCreationDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteLink(Guid linkId)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(linkId, requestId);
            var res = await _linkBusinessLogic.DeleteLink(linkId, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllLinks()
        {
            var requestId = Guid.NewGuid();
            RequestLogger(null, requestId);
            var res = await _linkBusinessLogic.GetAllLinksAsync(requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetLink([FromQuery] Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _linkBusinessLogic.GetLinkByIdAsync(id, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid id, [FromBody] LinkForUpdateDto linkForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(new { linkForUpdateDto, id }, requestId);
            var res = await _linkBusinessLogic.UpdateLinkAsync(id, linkForUpdateDto, requestId);
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