using System.Text.Json;
using InnovateFuture.Api.Models;
using InnovateFuture.Domain.Exceptions;
using InnovateFuture.Infrastructure.Exceptions;

namespace InnovateFuture.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new CommonResponse<object>
        {
            IsSuccess = false,
            Errors = new List<string> { exception.Message }
        };

        // Map exceptions to status codes
        context.Response.StatusCode = exception switch
        {
            FluentValidation.ValidationException validationException =>
                HandleValidationException(response, validationException),
            IFBusinessRuleViolationException => StatusCodes.Status400BadRequest,
            IFConcurrencyException => StatusCodes.Status409Conflict,
            IFDatabaseException => StatusCodes.Status500InternalServerError,
            IFDomainValidationException => StatusCodes.Status400BadRequest,
            IFEntityNotFoundException => StatusCodes.Status404NotFound,
            IFExternalServiceException => StatusCodes.Status503ServiceUnavailable,
            IFPolicyViolationException => StatusCodes.Status400BadRequest,
            IFUnauthorizedActionException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError // Catch-all for unhandled exceptions
        };

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static int HandleValidationException(CommonResponse<object> response, FluentValidation.ValidationException validationException)
    {
        response.Errors = validationException.Errors
            .Select(e => e.ErrorMessage)
            .Distinct()
            .ToList();
        return StatusCodes.Status400BadRequest;
    }
}