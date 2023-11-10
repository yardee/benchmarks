using System.Collections.Generic;
using BTDB.FieldHandler;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.LongLists;

public class ListContainer
{
    [PrimaryKey(1)]
    public required int Id { get; set; }
    public required ISet<int> Numbers { get; set; }
}


public interface IListContainerTable : IRelation<ListContainer>
{
    bool RemoveById(int id);
    ListContainer FindById(int id);
}

public class IndirectContainer
{
    [PrimaryKey(1)]
    public required int Id { get; set; }
    public required IIndirect<LazyStateItemIds> StoredItemIds { get; set; }
}

public class LazyStateItemIds
{
    public required ISet<int> ItemIds { get; set; }
}

public interface IIndirectContainerTable : IRelation<IndirectContainer>
{
    bool RemoveById(int id);
    IndirectContainer FindById(int id);
}