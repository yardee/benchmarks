using BTDB.ODBLayer;

namespace BtdbBenchmarks.EnumInObject;

public enum PersonState
{
    Single = 0,
    Divorced = 1,
    Dead = 2,
}

public class Person
{
    [PrimaryKey(1)] public int PersonId { get; set; }

    public object State { get; set; }
}

public interface IPersonTable : IRelation<Person>
{
    bool RemoveById(int personId);
    Person FindById(int personId);
}