using IssueTracker.Api.Middleware;

namespace IssueTracker.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder
        UseGlobalExceptionMiddleware(
            this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
