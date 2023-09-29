using BenchmarkDotNet.Running;
using BtdbBenchmarks.EnumInObject;
using BtdbBenchmarks.LongLists;
using BtdbBenchmarks.ManyRecords;
using BtdbBenchmarks.PartialViews;
using BtdbBenchmarks.UpdatesById;


namespace BtdbBenchmarks;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<LongListsBenchmark>();
        // var summary = BenchmarkRunner.Run<BtdbPartialBenchmark>();
        // var summary = BenchmarkRunner.Run<BtdbManyRecordsBenchmark>();
        // var summary = BenchmarkRunner.Run<BtdbUpdateByIdBenchmark>();
        // var bench = new BtdbUpdateByIdBenchmark();
        // bench.GlobalSetup();
        // bench.ClassicUpdate();
    }
}