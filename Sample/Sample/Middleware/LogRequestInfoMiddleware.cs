using Microsoft.AspNetCore.Http;
using Sample.Impl.Services.CustomLog;
using System;
using System.Threading.Tasks;

namespace Sample.Web.Middleware
{
    public class LogRequestInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICustomLogService logService)
        {
            var requestInfo = String.Format($"{context.Request.Method} {context.Request.Path}");

            logService.LogInformation(requestInfo);

            await _next(context);
        }
    }
}
