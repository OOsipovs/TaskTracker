using Microsoft.AspNetCore.Builder;
using TaskTracker.Shared.Middleware;

namespace TaskTracker.Shared.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}