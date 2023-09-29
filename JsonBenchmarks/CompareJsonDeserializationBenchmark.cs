using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

#nullable enable

namespace JsonBenchmarks;

[MemoryDiagnoser]
public class CompareJsonDeserializationBenchmark
{
    private List<string> _personsJsons = new List<string>();
    private string _personsJson;
    readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    [GlobalSetup]
    public void GlobalSetup()
    {
        var persons = new List<Person>();
        foreach (var parentId in Enumerable.Range(1, 10000))
        {
            var person = new Person(1, parentId, "Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent Parent " + parentId, 40)
            {
                Children = Enumerable.Range(0, 20).Select(i => new Person(parentId, i, "Child", 1)).ToList()
            };
            
            var json = JsonSerializer.Serialize(person, person.GetType(), new JsonSerializerOptions { WriteIndented = false });

            _personsJsons.Add(json);
            persons.Add(person);
            if (parentId == 1)
            {
                Console.WriteLine(json);
            }
        }
        
        _personsJson = JsonSerializer.Serialize(persons, persons.GetType(), new JsonSerializerOptions { WriteIndented = false });
    }
    
    [Benchmark]
    public List<Person> Newtonsoft()
    {
        var persons = new List<Person>();
        foreach (var personJson in _personsJsons)
        {
            persons.Add(JsonConvert.DeserializeObject<Person>(personJson, _jsonSerializerSettings)!);
        }

        return persons;
    }
    
    [Benchmark]
    public List<Person> JsonText_DeserializeEachItem()
    {
        var persons = new List<Person>();
        foreach (var personJson in _personsJsons)
        {
            persons.Add(JsonSerializer.Deserialize<Person>(personJson, JsonSerializerOptions.Default)!);
        }

        return persons;
    }
    
    [Benchmark]
    public List<Person> JsonText_DeserializeWholeList()
    {
        return JsonSerializer.Deserialize<List<Person>>(_personsJson, JsonSerializerOptions.Default)!;
    }
}