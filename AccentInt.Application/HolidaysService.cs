using AccentInt.Application.Exceptions;
using AccentInt.Application.Interfaces;
using AccentInt.Application.Models;
using AccentInt.Domain;
using System.Collections.Concurrent;

namespace AccentInt.Application;

public class HolidaysService(IClient client, ICache cache) 
    : IHolidayService
{
    private readonly IClient _client = client;
    private readonly ICache _cache = cache;
    public async Task<List<HolidayResponse>> GetDuplicatedHolidays(HolidaysRequest holidaysRequest)
    {
        var holidaysDict = new ConcurrentDictionary<DateTime, List<Holiday>>();

        var tasks = holidaysRequest.Countries.Select(async countryCode =>
        {
            Country country;
            if (await _cache.ExistsAsync(countryCode).ConfigureAwait(false))
            {
                country = (await _cache.GetCountryAsync(countryCode).ConfigureAwait(false))!;
            }
            else
            {
                country = await _client
                    .GetCountryHolidaysAsync(countryCode, holidaysRequest.Year!.Value)
                    .ConfigureAwait(false);
                
                await _cache.AddCountry(country).ConfigureAwait(false);
            }

            foreach (var holiday in country.Holidays)
            {
                if (holidaysDict.TryGetValue(holiday.Date, out List<Holiday>? value))
                {
                    value.Add(holiday);
                }
                else
                {
                    holidaysDict[holiday.Date] = [holiday];
                }
            }
        });

        await Task.WhenAll(tasks).ConfigureAwait(false);

        var duplicatedHolidays = holidaysDict
            .SelectMany(kvp => kvp.Value
                .GroupBy(h => h.Name, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g))
            .Select(hv => new HolidayResponse(hv.Date, hv.Name, hv.LocalName))
            .ToList();

        return duplicatedHolidays;
    }

    public async Task<List<HolidayCountResponse>> GetHolidaysDuringWorkDays(HolidaysRequest holidaysRequest)
    {
        var tasks = holidaysRequest.Countries.Select(async countryCode =>
        {
            if (await _cache.ExistsAsync(countryCode).ConfigureAwait(false))
            {
                var country = (await _cache.GetCountryAsync(countryCode).ConfigureAwait(false))!;
                var holidaysNumber = country
                    .GetHolidays(x => x.Date.DayOfWeek != DayOfWeek.Saturday && x.Date.DayOfWeek != DayOfWeek.Sunday)
                    .Count;
                return new HolidayCountResponse(countryCode, holidaysNumber);
            }

            var apiResponse = await _client.GetCountryHolidaysAsync(countryCode, holidaysRequest.Year!.Value).ConfigureAwait(false);

            await _cache.AddCountry(apiResponse).ConfigureAwait(false);

            var holidayNumber = apiResponse?.GetHolidays(x => x.Date.DayOfWeek != DayOfWeek.Saturday 
                        && x.Date.DayOfWeek != DayOfWeek.Sunday).Count ?? 0;

            return new HolidayCountResponse(countryCode, holidayNumber);
        });

        var toReturn = await Task.WhenAll(tasks).ConfigureAwait(false);

        return [.. toReturn.OrderByDescending(x => x.HolidayCount)];
    }

    public async Task<List<HolidayResponse>> GetLastThreeHolidays(HolidaysRequest holidaysRequest)
    {
        if (await _cache.ExistsAsync(holidaysRequest.Countries!.First()))
        {
            var country = (await _cache.GetCountryAsync(holidaysRequest.Countries!.First()))!;

            var filteredCountry = country.GetHolidays(h => h.Date <= DateTime.UtcNow)
                .OrderByDescending(x => x.Date)
                .Take(3);

            return [.. filteredCountry.Select(x => new HolidayResponse(x.Date, x.Name, x.LocalName))];
        }

        var apiResponse = await _client.GetCountriesHolidaysAsync(holidaysRequest.Countries, holidaysRequest.Year ?? DateTime.UtcNow.Year);
        if (apiResponse is null || apiResponse.Count == 0)
        {
            throw new HolidaysNotReturnedException("No holidays returned for the countries.");
        }

        var countryRes = apiResponse.First();

        await _cache.AddCountry(countryRes);

        if(countryRes.Holidays.Count < 3)
        {
            throw new CountryWithLessThanThreeHolidaysException("Not enough holidays to provide an answer");
        }
        var holidaysToReturn = countryRes.GetHolidays(h => h.Date <= DateTime.UtcNow)
            .OrderByDescending(x => x.Date)
            .Take(3);

        return [.. holidaysToReturn.Select(x => new HolidayResponse(x.Date, x.Name, x.LocalName))];
    }
}
