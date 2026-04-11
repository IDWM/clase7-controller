using System.Text.Json;
using clase7_controller.DTOs;
using clase7_controller.Exceptions;

namespace clase7_controller.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException exception)
        {
            await WriteErrorResponseAsync(
                context,
                (int)exception.StatusCode,
                exception.Message,
                exception.ErrorCode,
                exception.Errors
            );
        }
        catch (Exception)
        {
            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Ocurrió un error interno en el servidor.",
                "INTERNAL_ERROR",
                null
            );
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string message,
        string errorCode,
        IDictionary<string, string[]>? errors
    )
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        string traceId = context.TraceIdentifier;

        ApiErrorDataDto errorData = new()
        {
            ErrorCode = errorCode,
            TraceId = traceId,
            Errors = errors,
        };
        ApiResponse<ApiErrorDataDto> response = new() { Message = message, Data = errorData };

        string json = JsonSerializer.Serialize(response, _jsonSerializerOptions);
        await context.Response.WriteAsync(json);
    }
}
