using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentInt.Domain;

public class Country(string code, IList<Holiday> holidays)
{
    public string Code { get; } = code;
    public IList<Holiday> Holidays { get; } = holidays;

    public void AddHoliday(Holiday holiday)
    {
        if (Holidays == null)
        {
            throw new ArgumentNullException(nameof(Holidays));
        }

        if(Holidays.Any(Holidays => Holidays.Name == holiday.Name && Holidays.Date == holiday.Date))
        {
            return;
        }

        Holidays.Add(holiday);
    }

    public IList<Holiday> GetHolidays(Func<Holiday, bool>? filter = null, Func<Holiday, object>? orderBy = null)
    {
        if(Holidays.Count == 0 || filter == null)
        {
            return Holidays; 
        }

        var query = Holidays.Where(filter);

        if(orderBy != null)
        {
            query = query.OrderByDescending(orderBy);
        }

        return [.. query];
    }
}
