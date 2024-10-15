namespace EventsFactories.FactoryWithData;

static class PersonCreated3Factory
{
    public static PersonCreated3 Create(CreatePersonRequest request, PersonCreated3Data data) =>
        new()
        {
            Id = data.Id,
            Name = request.Name,
            Date = data.Date,
            SomeValue = data.SomeValue
        };
}