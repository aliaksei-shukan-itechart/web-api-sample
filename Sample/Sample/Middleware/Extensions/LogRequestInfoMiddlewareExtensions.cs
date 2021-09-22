using Microsoft.AspNetCore.Builder;

namespace Sample.Web.Middleware.Extensions
{
    public static class LogRequestInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestInfoLogging(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogRequestInfoMiddleware>();
        }
    }
}
