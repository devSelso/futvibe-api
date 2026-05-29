using System.Net;
using System.Text.Json;
using FluentValidation;
using Futvibe.Domain.Exceptions;

namespace Futvibe.WebApi.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleAsync(context, ex);
        }
    }

    private static async Task HandleAsync(HttpContext context, Exception ex)
    {
        var (statusCode, error, details) = ex switch
        {
            NotFoundException e      => (HttpStatusCode.NotFound, e.Message, (IEnumerable<string>?)null),
            ForbiddenException e     => (HttpStatusCode.Forbidden, e.Message, null),
            BusinessException e      => (HttpStatusCode.UnprocessableEntity, e.Message, null),
            ValidationException e    => (HttpStatusCode.BadRequest, "Dados inválidos.",
                                         e.Errors.Select(f => f.ErrorMessage)),
            _                        => (HttpStatusCode.InternalServerError, "Erro interno.", null)
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var body = new { error, statusCode = (int)statusCode, details };
        await context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}
