namespace EventsFactories.FactoryWithParams;

public class PersonCreated2
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Date { get; set; }
    
    public ulong? SomeValue { get; set; }
}