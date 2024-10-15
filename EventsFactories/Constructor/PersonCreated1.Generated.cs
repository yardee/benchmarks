using System.Diagnostics.CodeAnalysis;

namespace EventsFactories.Constructor;

public partial class PersonCreated1
{
    [SetsRequiredMembers]
    public PersonCreated1(CreatePersonRequest request, int id, DateTime date)
    {
        Id = id;
        Name = request.Name;
        Date = date;
    }
}
