using System.Collections.Generic;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.UpdatesById;


public enum PersonState
{
    Single = 0,
    Divorced = 1,
    Dead = 2,
}

public class Person
{
    [PrimaryKey(1)]
    public int ParentId { get; set; }

    [PrimaryKey(2)]
    public int PersonId { get; set; }

    public string Name { get; set; }

    public ulong Age { get; set; }

    public PersonState State { get; set; }

    public IList<Person>? Children { get; set; }
    
    public object Test { get; set; }

    public Person(int parentId, int personId, string name, ulong age)
    {
        ParentId = parentId;
        PersonId = personId;
        Name = name;
        Age = age;
        State = PersonState.Single;
    }
}

public class PersonOnlyId
{
    public int ParentId { get; set; }
    public int PersonId { get; set; }
}

public class PersonOnlyName: PersonOnlyId
{
    public string Name { get; set; }
}

public interface IPersonTable : IRelation<Person>
{
    bool RemoveById(int parentId, int personId);
    IEnumerable<Person> FindById(int parentId);
    Person FindById(int parentId, int personId);
    bool UpdateById(int parentId, int personId, PersonState state);
    IEnumerable<PersonOnlyId> FindByIdOnlyId(int parentId);
    IEnumerable<PersonOnlyName> FindByIdOnlyName(int parentId);
}

public class Tests {
    public string Name { get; set; }}