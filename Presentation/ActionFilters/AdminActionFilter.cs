using System;
using System.Threading.Tasks;
using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kenaret.ActionFilters
{
    public class AdminActionFilter : IActionFilter
    {
        private readonly IJwtTokenCreator _jwtTokenCreator;

        public AdminActionFilter(IJwtTokenCreator jwtTokenCreator)
        {
            _jwtTokenCreator = jwtTokenCreator;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var isAdmin = _jwtTokenCreator.IsAdminToken(token);
            if (isAdmin is not false) return;
            context.Result = new ForbidResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}