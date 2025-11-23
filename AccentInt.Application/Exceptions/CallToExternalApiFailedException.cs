namespace AccentInt.Application.Exceptions;

public class CallToExternalApiFailedException(string message, Exception? ex = null)
    : Exception(message, ex);
