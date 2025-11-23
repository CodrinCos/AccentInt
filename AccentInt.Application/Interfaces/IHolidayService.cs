using AccentInt.Application.Models;
using AccentInt.Domain;

namespace AccentInt.Application.Interfaces;

public interface IHolidayService
{
    public Task<List<HolidayResponse>> GetLastThreeHolidays(HolidaysRequest holidaysRequest); 
    public Task<List<HolidayCountResponse>> GetHolidaysDuringWorkDays(HolidaysRequest holidaysRequest);
    public Task<List<HolidayResponse>> GetDuplicatedHolidays(HolidaysRequest holidaysRequest);
}
