using BenchmarkDotNet.Attributes;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

#nullable enable

namespace BtdbBenchmarks.UpdatesById;

[MemoryDiagnoser]
public class BtdbUpdateByIdBenchmark
{
    private IKeyValueDB _keyValueDb = null!;
    private IObjectDB _db = null!;
    private Func<IObjectDBTransaction, IPersonTable> _personsCreator = null!;

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
        _personsCreator = tr.InitRelation<IPersonTable>("Person");
        var personTable = _personsCreator(tr);

        foreach (var parentId in Enumerable.Range(1, 10000))
        {
            var person = new Person(1, parentId, "Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent " + parentId, 40)
            {
                Children = Enumerable.Range(0, 100).Select(i => new Person(parentId, i, "Child", 1)).ToList(),
                Test = new Tests{ Name = "BLa"}
            };
            personTable.Upsert(person);
        }

        tr.Commit();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);
        foreach (var person in personTable.ToList())
        {
            personTable.RemoveById(person.ParentId, person.PersonId);
        }

        tr.Commit();
        _db.Dispose();
    }

    [Benchmark]
    public ulong ClassicUpdate()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);

        var count = 0ul;
            
        foreach (var parentId in Enumerable.Range(1, 100))
        {
            var person = personTable.FindById(1, parentId);
            person.State = PersonState.Dead;
            personTable.Upsert(person);
            count++;
        }

        return count;
    }

    [Benchmark]
    public ulong UpdateById()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);

        var count = 0ul;
            
        foreach (var parentId in Enumerable.Range(1, 100))
        {
            personTable.UpdateById(1, parentId, PersonState.Dead);
            count++;
        }

        return count;
    }
}