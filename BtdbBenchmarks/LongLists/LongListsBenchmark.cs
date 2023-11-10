using BenchmarkDotNet.Attributes;
using BTDB.FieldHandler;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using BtdbBenchmarks.LongLists;

namespace BtdbBenchmarks.LongLists;

[MemoryDiagnoser]
public class LongListsBenchmark
{
    private IKeyValueDB _keyValueDb = null!;
    private IObjectDB _db = null!;
    private Func<IObjectDBTransaction, IListContainerTable> _listCreator = null!;
    private Func<IObjectDBTransaction, IIndirectContainerTable> _indirectCreator = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Directory.CreateDirectory("db");
        _keyValueDb = new BTreeKeyValueDB(new OnDiskFileCollection("db"));
        _db = new ObjectDB();

        var options = new DBOptions()
            .WithSelfHealing();

        _db.Open(_keyValueDb, true, options);

        using var tr = _db.StartTransaction();
        _listCreator = tr.InitRelation<IListContainerTable>("List");
        _indirectCreator = tr.InitRelation<IIndirectContainerTable>("Indirect");
        var listTable = _listCreator(tr);
        var indirectTable = _indirectCreator(tr);
        var ids = Enumerable.Range(1, 100000).ToHashSet();
        
        foreach (var id in Enumerable.Range(1, 10))
        {
            listTable.Upsert(new ListContainer
            {
                Id = id,
                Numbers = ids.ToHashSet(),
            });
            
            indirectTable.Upsert(new IndirectContainer
            {
                Id = id,
                StoredItemIds = new DBIndirect<LazyStateItemIds>(new LazyStateItemIds { ItemIds = ids.ToHashSet() }),
            });
        }

        tr.Commit();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        using var tr = _db.StartTransaction();
        var listTable = _listCreator(tr);
        foreach (var i in listTable.ToList())
        {
            listTable.RemoveById(i.Id);
        }
        
        var indirectTable = _indirectCreator(tr);
        foreach (var i in indirectTable.ToList())
        {
            indirectTable.RemoveById(i.Id);
        }

        tr.Commit();
        _db.Dispose();
    }

    [Benchmark]
    public ulong Insert_1000_List()
    {
        var count = 0ul;
            
        foreach (var num in Enumerable.Range(100000, 1000))
        {
            using var tr = _db.StartTransaction();
            var table = _listCreator(tr);
            var obj = table.FindById(1);
            obj.Numbers.Add(num);
            table.Upsert(obj);
            count++;
            tr.Commit();
        }

        return count;
    }
    
    [Benchmark]
    public ulong Insert_1000_Indirect()
    {
        var count = 0ul;
            
        foreach (var num in Enumerable.Range(100000, 1000))
        {
            using var tr = _db.StartTransaction();
            var table = _indirectCreator(tr);
            var obj = table.FindById(1);
            obj.StoredItemIds.Value.ItemIds.Add(num);
            tr.Store(obj.StoredItemIds);
            count++;
            tr.Commit();
        }

        return count;
    }
}