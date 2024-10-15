using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using EagleArchiveTests.CommunicationPiece.MetadataGeneration;
using JsonBenchmarks.Dto;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

#nullable enable

namespace JsonBenchmarks;

[JsonSourceGenerationOptions(WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(Metadata))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

[MemoryDiagnoser]
public class CompareJsonSourceGeneratorSerializationBenchmark
{
    private Metadata _metadata = default!;
    private string _metadataJson = default!;
    private readonly MetadataGenerator _metadataGenerator = new();
    private readonly JsonSerializerOptions _options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _metadata = _metadataGenerator.Next();
        _metadataJson = JsonSerializer.Serialize(_metadata);
    }
    
    [Benchmark]
    public string SourceGenerator_Serialize()
    {
        return JsonSerializer.Serialize(_metadata, typeof(Metadata), SourceGenerationContext.Default);
    }
    
    [Benchmark]
    public string Classic_Serialize()
    {
        return JsonSerializer.Serialize(_metadata, _options);
    }
    
    [Benchmark]
    public Metadata SourceGenerator_Deserialize()
    {
        return (Metadata)JsonSerializer.Deserialize(_metadataJson, typeof(Metadata), SourceGenerationContext.Default)!;
    }
    
    [Benchmark]
    public Metadata Classic_Deserialize()
    {
        return (Metadata)JsonSerializer.Deserialize(_metadataJson, typeof(Metadata), _options)!;
    }
}