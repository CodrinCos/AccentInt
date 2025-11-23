namespace AccentInt.Domain;

public class Holiday(string name, string localName, DateTime date)
{
    public string Name { get; } = name;
    public string LocalName { get; } = localName;
    public DateTime Date { get; } = date;
}
