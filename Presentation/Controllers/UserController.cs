using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.User;
using Contracts.LoggerManager;
using Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _userBusinessLogic;
        private readonly ILoggerManager _loggerManager;
        private readonly EventHandlersManager _eventHandlersManager;

        public UserController(IUserBusinessLogic userBusinessLogic, ILoggerManager loggerManager, EventHandlersManager eventHandlersManager)
        {
            _userBusinessLogic = userBusinessLogic;
            _loggerManager = loggerManager;
            _eventHandlersManager = eventHandlersManager;
        }

        [HttpPost("register"), Authorize]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(userForRegistrationDto, requestId);
            var userName = HttpContext.User.Identity.Name;
            var res = await _userBusinessLogic.RegisterUserAsync(userForRegistrationDto, userName, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetUserByUsername()
        {
            var requestId = Guid.NewGuid();
            RequestLogger(null, requestId);
            var userName = HttpContext.User.Identity.Name;
            var res = await _userBusinessLogic.GetUserAsync(userName, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPost("phonenumberauthentication")]
        public async Task<IActionResult> PhoneNumberAuthentication([FromBody] UserForPhoneAuthDto userForPhoneAuthDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(userForPhoneAuthDto, requestId);
            var res = await _userBusinessLogic.PhoneNumberAuthenticationAsync(userForPhoneAuthDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPost("otpcodeauthentication")]
        public async Task<IActionResult> OtpCodeAuthentication([FromBody] UserForOtpAuthDto userForOtpAuthDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(userForOtpAuthDto, requestId);
            var res = await _userBusinessLogic.OtpCodeAuthenticationAsync(userForOtpAuthDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensForRefreshDto tokensForRefreshDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(tokensForRefreshDto, requestId);
            var res = await _userBusinessLogic.RefreshAsync(tokensForRefreshDto, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UserForUpdateDto userForUpdateDto)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(userForUpdateDto, requestId);
            var userName = HttpContext.User.Identity.Name;
            var res = await _userBusinessLogic.UpdateUserAsync(userForUpdateDto, userName, requestId);
            _loggerManager.LogDebug("Response Body: " + JsonConvert.SerializeObject(res), requestId, true);
            return res.Success ? Ok(res) : StatusCode(500, res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileInfo(Guid id)
        {
            var requestId = Guid.NewGuid();
            RequestLogger(id, requestId);
            var res = await _userBusinessLogic.GetProfileInfoAsync(id, requestId);
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