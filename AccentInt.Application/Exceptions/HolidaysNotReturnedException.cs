namespace AccentInt.Application.Exceptions;

public class HolidaysNotReturnedException(string message, Exception? ex = null) 
    : Exception(message, ex);