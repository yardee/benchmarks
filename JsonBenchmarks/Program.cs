// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using JsonBenchmarks;

// var summary = BenchmarkRunner.Run<CompareJsonSerializationBenchmark>();
// var summary = BenchmarkRunner.Run<CompareJsonDeserializationBenchmark>();
var summary = BenchmarkRunner.Run<CompareJsonSourceGeneratorSerializationBenchmark>();