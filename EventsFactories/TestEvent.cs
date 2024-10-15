namespace EventsFactories;

public class TestEvent
{
    public required string Name { get; set; }
}

public static class TestEventFactory
{
    public static TestEvent Create(CreatePersonRequest request)
    {
        return new TestEvent
        {
            Name = request.Name
        };
    }
}