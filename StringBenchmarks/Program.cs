// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using StringBenchmarks;


var summary = BenchmarkRunner.Run<CompareStringCreation>();