namespace AccentInt.Application.Models;

public class HolidayCountResponse(string countryCode, int holidayCount)
{
    public string? CountryCode { get; set; } = countryCode;
    public int? HolidayCount { get; set; } = holidayCount;
}
