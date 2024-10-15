namespace EventsFactories.FactoryWithParams;

static class PersonCreated2Factory
{
    public static PersonCreated2 Create(CreatePersonRequest request, int id, DateTime date) =>
        new()
        {
            Id = id,
            Name = request.Name,
            Date = date
        };
}