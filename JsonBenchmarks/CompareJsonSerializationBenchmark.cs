using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

#nullable enable

namespace JsonBenchmarks;

[MemoryDiagnoser]
public class CompareJsonSerializationBenchmark
{
    private List<Person> _persons = new List<Person>();
    private DirectoryInfo _directory = Directory.CreateDirectory(Path.Combine("persons"));
    readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    [GlobalSetup]
    public void GlobalSetup()
    {
        foreach (var parentId in Enumerable.Range(1, 500))
        {
            _persons.Add(new Person(1, parentId, "Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent " + parentId, 40)
            {
                Children = Enumerable.Range(0, 20).Select(i => new Person(parentId, i, "Child", 1)).ToList()
            });
        }
    }
    
    [Benchmark]
    public async Task Newtonsoft()
    {
        var subDir = Guid.NewGuid().ToString();
        _directory.CreateSubdirectory(subDir);
        var filePath = Path.Combine(_directory.FullName, subDir, "textJson.json");
        await using var stream = File.Create(filePath);
        var writer = new StreamWriter(stream, Encoding.UTF8);
        await writer.WriteAsync(JsonConvert.SerializeObject(_persons, _jsonSerializerSettings));
        await writer.FlushAsync();
    }
    
    [Benchmark]
    public async Task JsonText()
    {
        var subDir = Guid.NewGuid().ToString();
        _directory.CreateSubdirectory(subDir);
        var filePath = Path.Combine(_directory.FullName, subDir, "newtonsoft.json");
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, _persons, _persons.GetType(), new JsonSerializerOptions { WriteIndented = false });
    }
}