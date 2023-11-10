using System.Collections.Generic;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.PartialViews;

public class Person
{
    [PrimaryKey(1)] 
    public required int ParentId { get; set; }

    [PrimaryKey(2)]
    public required int PersonId { get; set; }

    public required string Name { get; set; }

    public required ulong Age { get; set; }

    public IList<Person>? Children { get; set; }
}

public class PersonOnlyId
{
    public int ParentId { get; set; }
    public int PersonId { get; set; }
}

public class PersonOnlyName: PersonOnlyId
{
    public required string Name { get; set; }
}

public interface IPersonTable : IRelation<Person>
{
    bool RemoveById(int parentId, int personId);
    IEnumerable<Person> FindById(int parentId);
    IEnumerable<PersonOnlyId> FindByIdOnlyId(int parentId);
    IEnumerable<PersonOnlyName> FindByIdOnlyName(int parentId);
}