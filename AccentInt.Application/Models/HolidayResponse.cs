namespace AccentInt.Application.Models;

public class HolidayResponse(DateTime dateTime, string name, string localName)
{
    public DateTime Date { get; set; } = dateTime;
    public string Name { get; set; } = name;
    public string LocalName { get; set; } = localName;
}
