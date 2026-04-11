using System.Net;

namespace clase7_controller.Exceptions;

public abstract class AppException(
    string message,
    HttpStatusCode statusCode,
    string errorCode,
    IDictionary<string, string[]>? errors = null
) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public string ErrorCode { get; } = errorCode;
    public IDictionary<string, string[]>? Errors { get; } = errors;
}
