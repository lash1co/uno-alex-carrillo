using IssueTracker.Domain.Exceptions;
using System.Net;

namespace IssueTracker.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                exception.Message);

            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType =
            "application/json";

        context.Response.StatusCode =
            exception switch
            {
                BadRequestException =>
                    (int)HttpStatusCode.BadRequest,

                NotFoundException =>
                    (int)HttpStatusCode.NotFound,

                UnauthorizedAccessException =>
                    (int)HttpStatusCode.Unauthorized,

                _ =>
                    (int)HttpStatusCode.InternalServerError
            };

        var response = new
        {
            message = exception.Message
        };

        await context.Response.WriteAsJsonAsync(
            response);
    }
}