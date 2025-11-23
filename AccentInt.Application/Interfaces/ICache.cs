using AccentInt.Domain;

namespace AccentInt.Application.Interfaces;

public interface ICache
{
    Task<bool> ExistsAsync(string countryCode);
    Task<Country?> GetCountryAsync(string countryCode);
    Task AddCountry(Country country);
}
