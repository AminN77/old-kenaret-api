using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Feedback;
using Contracts.LoggerManager;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackBusinessLogic _feedbackBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;

        public FeedbackController(IFeedbackBusinessLogic feedbackBusinessLogic, ILoggerManager loggerManager, EventHandlersManager eventHandlersManager)
        {
            _feedbackBusinessLogic = feedbackBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = eventHandlersManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackForCreationDto feedbackForCreationDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(feedbackForCreationDto, requestId);
            var res = await _feedbackBusinessLogic.CreateFeedbackAsync(feedbackForCreationDto, requestId);
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