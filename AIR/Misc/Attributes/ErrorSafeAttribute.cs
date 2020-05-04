using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Misc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ErrorSafeAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;

            filterContext.HttpContext.Response.StatusCode = 400;
            filterContext.HttpContext.Response.ContentType = "application/json";
            filterContext.HttpContext.Response.WriteAsync(filterContext.Exception.ToString());
            filterContext.ExceptionHandled = true;
        }
    }
}

    