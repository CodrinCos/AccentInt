using AccentInt.Domain;

namespace AccentInt.Application.Interfaces;

public interface IClient
{
    Task<List<Country>> GetCountriesHolidaysAsync(List<string> countryCodes, int year);
    Task<Country> GetCountryHolidaysAsync(string countryCode, int year);
}
