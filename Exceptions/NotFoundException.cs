using System.Net;

namespace clase7_controller.Exceptions;

public class NotFoundException(string message)
    : AppException(message, HttpStatusCode.NotFound, "NOT_FOUND");
