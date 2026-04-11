using System.Net;

namespace clase7_controller.Exceptions;

public class PersistenceException(string message)
    : AppException(message, HttpStatusCode.InternalServerError, "PERSISTENCE_ERROR");
