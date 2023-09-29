// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LambdaMemory;

// var summary = BenchmarkRunner.Run<CompareLambdaAllocations>();

var a = new CompareLambdaAllocations();

await a.VariableAwaiterInstance();