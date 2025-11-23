using AccentInt.Domain;
using AccentInt.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.Metrics;

namespace AccentInt.Infrastructure;

public class Cache(IMemoryCache memoryCache) : ICache
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public Task AddCountry(Country country)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        if (_memoryCache.TryGetValue(country.Code, out _))
        {
            _memoryCache.Remove(country.Code);
        }

        _memoryCache.Set(country.Code, country, cacheOptions);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string countryCode)
    {
        if(_memoryCache.TryGetValue(countryCode, out IList<Country>? _))
        {
            Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<IList<Country>> GetCountryAsync(string countryCode)
    {
        if (_memoryCache.TryGetValue(countryCode, out IList<Country>? country))
        {
            if(country is null)
            {
                return Task.FromResult<IList<Country>>([]);
            }

            return Task.FromResult(country);
        }

        return Task.FromResult<IList<Country>>([]);
    }
}
