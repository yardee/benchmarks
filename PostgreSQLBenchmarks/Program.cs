// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using PostgreSQLBenchmarks.PrimaryKeys;
var config = DefaultConfig.Instance
    .AddJob(Job
        .MediumRun
        .WithLaunchCount(1)
        .WithToolchain(InProcessNoEmitToolchain.Instance));

var summary = BenchmarkRunner.Run<PrimaryKeysBenchmark>(config);

// var primaryKeysBenchmark = new PrimaryKeysBenchmark();
//
// try
// {
//     primaryKeysBenchmark.GlobalSetup();
//     foreach (var recordWithSinglePrimaryKey in primaryKeysBenchmark.RecordWithSinglePrimaryKeys())
//     {
//         Console.WriteLine(recordWithSinglePrimaryKey.Name);
//     }
// }
// finally
// {
//     primaryKeysBenchmark.Cleanup();
// }