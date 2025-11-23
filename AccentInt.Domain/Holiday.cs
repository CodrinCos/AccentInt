namespace AccentInt.Domain;

public class Holiday(string name, DateTime date)
{
    public string Name { get; } = name;
    public DateTime Date { get; } = date;
}
