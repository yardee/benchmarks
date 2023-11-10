using BenchmarkDotNet.Attributes;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.PartialViews;

[MemoryDiagnoser]
public class BtdbPartialBenchmark
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
            personTable.Upsert(new Person
            {
                PersonId = 1,
                ParentId = parentId,
                Name = "Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent " + parentId,
                Age = 40,
                Children = Enumerable.Range(0, 100).Select(i => new Person
                {
                    ParentId = parentId, PersonId = i, Name = "Child", Age = 1
                }).ToList()
            });
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
    public ulong WholeUsers()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);

        var count = 0ul;
            
        foreach (var person in personTable.FindById(1))
        {
            count += (ulong)person.PersonId;
        }

        return count;
    }

    [Benchmark]
    public ulong OnlyPrimaryKeys()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);
            
        var count = 0ul;
            
        foreach (var person in personTable.FindByIdOnlyId(1))
        {
            count += (ulong)person.PersonId;
        }

        return count;
    }

    [Benchmark]
    public ulong GetAllUsersOnlyName()
    {
        using var tr = _db.StartTransaction();
        var personTable = _personsCreator(tr);

        var count = 0ul;
            
        foreach (var person in personTable.FindByIdOnlyName(1))
        {
            count += (ulong)person.PersonId;
        }

        return count;
    }
}