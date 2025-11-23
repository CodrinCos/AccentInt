using AccentInt.Application.Exceptions;
using AccentInt.Application.Interfaces;
using AccentInt.Domain;
using System.Net.Http.Json;

namespace AccentInt.Infrastructure;

public class DateNagerClient(HttpClient httpClient) : IClient
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<List<Country>> GetCountriesHolidaysAsync(List<string> countryCodes, int year)
    {
        var countriesReponse = countryCodes.Select(async countryCode =>
        {
            try
            {
                var response = await _httpClient.GetAsync($"PublicHolidays/{year}/{countryCode}");
                response.EnsureSuccessStatusCode();

                var holidays = await response.Content.ReadFromJsonAsync<List<Holiday>>();

                holidays ??= [];

                return new Country(countryCode, holidays);
            }
            catch (HttpRequestException ex)
            {
                throw new CallToExternalApiFailedException($"Failed to get holidays from DateNager external API due to: {ex.Message}", ex);
            }

        });

        return [.. await Task.WhenAll(countriesReponse)];
    }

    public async Task<Country> GetCountryHolidaysAsync(string countryCode, int year)
    {
        try
        {
            var response = await _httpClient.GetAsync($"PublicHolidays/{year}/{countryCode}");
            response.EnsureSuccessStatusCode();

            var holidays = await response.Content.ReadFromJsonAsync<List<Holiday>>();

            holidays ??= [];

            return new Country(countryCode, holidays);
        }
        catch (HttpRequestException ex)
        {
            throw new CallToExternalApiFailedException($"Failed to get holidays from DateNager external API due to: {ex.Message}", ex);
        }
    }
}
