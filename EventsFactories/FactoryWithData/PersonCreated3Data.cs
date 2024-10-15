namespace EventsFactories.FactoryWithData;

record struct PersonCreated3Data
{
    public required int Id { get; set; }
    public required DateTime Date { get; set; }
    public ulong? SomeValue { get; set; }
}
