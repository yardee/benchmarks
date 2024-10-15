namespace EventsFactories.LegacyEnrich;

public class PersonCreated4
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime Date { get; set; }
    
    public ulong? SomeValue { get; set; }
}