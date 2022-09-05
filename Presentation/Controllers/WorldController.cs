using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using Contracts.LoggerManager;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers
{
    [ApiController]
    [Route("api/worlds")]


    public class WorldController : ControllerBase
    {

        private readonly IWorldBusinessLogic _worldBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;

        public WorldController(IWorldBusinessLogic worldBusinessLogic, ILoggerManager loggerManager, EventHandlersManager eventHandlersManager)
        {
            _worldBusinessLogic = worldBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = eventHandlersManager;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllWorlds()
        {
            var requestId = Guid.NewGuid();
            RequestLogger(null, requestId);
            var res = await _worldBusinessLogic.GetAllWorldsAsync(requestId);
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