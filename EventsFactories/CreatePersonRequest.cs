using EventsFactories.Constructor;
using EventsFactories.FactoryWithData;
using EventsFactories.FactoryWithParams;
using EventsFactories.LegacyEnrich;

namespace EventsFactories;

public class CreatePersonRequest
{
    public required string Name { get; set; }
    public required string Val1 { get; set; }
    public required string Val3 { get; set; }
    public required ulong Val4 { get; set; }
    public required ulong Val5 { get; set; }
    public required int Val6 { get; set; }
    public required int Val7 { get; set; }
    public required string Val8 { get; set; }
    public required string Val9 { get; set; }
    public required string DiffVal1 { get; set; }
    public required string DiffVal2 { get; set; }
    public required string DiffVal3 { get; set; }

    public TestEvent CreateDefault() => TestEventFactory.Create(this);

    public PersonCreated1 CreateWithConstructorWithParams()
    {
        return new PersonCreated1(this, 5, DateTime.UtcNow)
        {
            SomeValue = 156
        };
    }
    
    public PersonCreated2 CreateWithFactoryWithParams()
    {
        var ev = PersonCreated2Factory.Create(this, 5, DateTime.UtcNow);
        ev.SomeValue = 156;
        return ev;
    }
    
    public PersonCreated3 CreateWithFactoryWithData()
    {
        var eventData = new PersonCreated3Data
        {
            Date = DateTime.UtcNow,
            Id = 5,
            SomeValue = 156
        };
        
        return PersonCreated3Factory.Create(this, eventData);
    }
    
    public PersonCreated4 CreateWithLegacyEnricher(GenericEnricher enricher)
    {
        var ev = new PersonCreated4();
        enricher.EnrichProperties(this, ev);
        ev.SomeValue = 156;
        ev.Date = DateTime.UtcNow;
        ev.Id = 5;
       
        return ev;
    }
}