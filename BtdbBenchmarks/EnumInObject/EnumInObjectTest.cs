using BenchmarkDotNet.Attributes;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

namespace BtdbBenchmarks.EnumInObject;

[MemoryDiagnoser]
public class EnumInObjectTest
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

        foreach (var parentId in Enumerable.Range(1, 1))
        {
            var person = new Person
            {
                PersonId = parentId,
                State = "5"
            };
            personTable.Upsert(person);
        }

        tr.Commit();
    }
}