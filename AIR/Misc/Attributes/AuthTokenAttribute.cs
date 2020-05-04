using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Misc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthTokenAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private const string AuthToken = "QHzNZvsFzqvhy/VcURXzLqrRliFwRJQ+puZQ0a9xBWE=";
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
            
            var tokenString = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(tokenString) || !tokenString.Equals(AuthToken)) context.Result = new UnauthorizedResult();
            else context.Result = null;
        }
    }
}