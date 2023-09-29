using BenchmarkDotNet.Running;
using CollectionBenchmarks;

var summary = BenchmarkRunner.Run<CompareCollections>();