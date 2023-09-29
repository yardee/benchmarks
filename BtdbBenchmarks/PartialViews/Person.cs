using System.Collections.Generic;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.PartialViews;

public class Person
{
    [PrimaryKey(1)]
    public int ParentId { get; set; }

    [PrimaryKey(2)]
    public int PersonId { get; set; }

    public string Name { get; set; }

    public ulong Age { get; set; }

    public IList<Person>? Children { get; set; }

    public Person(int parentId, int personId, string name, ulong age)
    {
        ParentId = parentId;
        PersonId = personId;
        Name = name;
        Age = age;
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
    IEnumerable<PersonOnlyId> FindByIdOnlyId(int parentId);
    IEnumerable<PersonOnlyName> FindByIdOnlyName(int parentId);
}