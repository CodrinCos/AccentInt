namespace AccentInt.Application.Exceptions;

public class CountryWithLessThanThreeHolidaysException(string message, Exception? ex = null)
    : Exception(message, ex);
