using System.Net;

namespace clase7_controller.Exceptions;

public class ValidationException(string message, IDictionary<string, string[]> errors)
    : AppException(message, HttpStatusCode.BadRequest, "VALIDATION_ERROR", errors);
