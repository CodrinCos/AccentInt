using AccentInt.Domain;

namespace AccentInt.Infrastructure.Interfaces;

public interface ICache
{
    Task<bool> ExistsAsync(string countryCode);
    Task<IList<Country>> GetCountryAsync(string countryCode);
    Task AddCountry(Country country);
}
