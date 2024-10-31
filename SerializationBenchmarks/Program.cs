// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using SerializationBenchmarks;

var summary = BenchmarkRunner.Run<CompareSerializersBenchmark>();

// var benchrmark = new CompareSerializersBenchmark();
// benchrmark.GlobalSetup();
// var json = benchrmark.JSON_Serialize();
// var jsonCompressed = benchrmark.JSON_Compressed_Serialize();
// var serializer = benchrmark.Serializer_Serialize();
// var serializerCompressed = benchrmark.Serializer_Compressed_Serialize();
//
// Console.WriteLine($"json: {json.Length}");
// Console.WriteLine($"jsonCompressed: {jsonCompressed.Length}");
// Console.WriteLine($"serializer: {serializer.Length}");
// Console.WriteLine($"serializerCompressed: {serializerCompressed.Length}");
//
// var obj3 = benchrmark.JSON_Deserialize();
// var obj4 = benchrmark.JSON_Compressed_Deserialize();
// var obj1 = benchrmark.Serializer_Compressed_Deserialize();
// var obj2 = benchrmark.Serializer_Deserialize();
//
// Console.WriteLine("Done");

