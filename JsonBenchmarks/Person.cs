#nullable enable

namespace JsonBenchmarks;

public enum PersonState
{
    Single = 0,
    Divorced = 1,
    Dead = 2,
}


public class Person
{
    public int ParentId { get; set; }

    public int PersonId { get; set; }

    public string Name { get; set; }

    public ulong Age { get; set; }

    public PersonState State { get; set; }

    public IList<Person>? Children { get; set; }

    public Person(int parentId, int personId, string name, ulong age)
    {
        ParentId = parentId;
        PersonId = personId;
        Name = name;
        Age = age;
        State = PersonState.Single;
    }
}