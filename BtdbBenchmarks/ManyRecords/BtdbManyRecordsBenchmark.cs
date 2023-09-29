using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

#nullable enable

namespace BtdbBenchmarks.ManyRecords;

[MemoryDiagnoser]
public class BtdbManyRecordsBenchmark
{
    private IKeyValueDB _keyValueDb = null!;
    private IObjectDB _db = null!;
    private Func<IObjectDBTransaction, ICommunicationPieceMetadataTable> _metadataCreator = null!;

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
        _metadataCreator = tr.InitRelation<ICommunicationPieceMetadataTable>("CommunicationPieceMetadata");
        var metadataTable = _metadataCreator(tr);

        foreach (var id in Enumerable.Range(1, 4000000))
        {
            var companyId = (ulong) id;
            metadataTable.Upsert(new CommunicationPieceMetadata(companyId, companyId * 10, $"{id};NA;26RK;C0YKT", DateTime.MinValue.AddSeconds(id))
            {
                Files = Enumerable.Range(1, 10).Select(i => MetadataFile.ProcessedAckFile("some path", i.ToString())).ToList()
            });
        }

        tr.Commit();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        using var tr = _db.StartTransaction();
        var metadataTable = _metadataCreator(tr);
        foreach (var person in metadataTable.ToList())
        {
            metadataTable.RemoveById(person.CompanyId, person.CommunicationPieceId);
        }

        tr.Commit();
        _db.Dispose();
    }

    [Benchmark]
    public ulong FilterDates()
    {
        using var tr = _db.StartTransaction();
        var metadataTable = _metadataCreator(tr);

        var count = 0ul;
        var date = DateTime.MinValue.AddSeconds(1000000);
        foreach (var metadata in metadataTable.Where(p => p.CreatedDate < date))
        {
            count += (ulong)metadata.CompanyId;
        }

        return count;
    }
}