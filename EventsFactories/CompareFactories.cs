using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using EventsFactories.Constructor;
using EventsFactories.FactoryWithData;
using EventsFactories.FactoryWithParams;
using EventsFactories.LegacyEnrich;

namespace EventsFactories;

[MemoryDiagnoser]
public class CompareFactories
{
    CreatePersonRequest _request = default!;
    CreatePersonRequest[] _requests = default!;
    private GenericEnricher _enricher = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _enricher = new();
        _request = new CreatePersonRequest
        {
            Name = "John",
            Val1 = "1",
            Val3 = "null",
            Val4 = 0,
            Val5 = 0,
            Val6 = 0,
            Val7 = 0,
            Val8 = "null",
            Val9 = "null",
            DiffVal1 = "null",
            DiffVal2 = "null",
            DiffVal3 = "null",
        };

        _requests = Enumerable.Range(0, 10000).Select(i => new CreatePersonRequest
        {
            Name = $"Name_{i}",
            Val1 = "1",
            Val3 = "null",
            Val4 = 0,
            Val5 = 0,
            Val6 = 0,
            Val7 = 0,
            Val8 = "null",
            Val9 = "null",
            DiffVal1 = "null",
            DiffVal2 = "null",
            DiffVal3 = "null",
        }).ToArray();
    }

    // [Benchmark]
    public PersonCreated1 ConstructorWithParams()
    {
        return _request.CreateWithConstructorWithParams();
    }
    
    // [Benchmark]
    public PersonCreated2 CreateWithFactoryWithParams()
    {
        return _request.CreateWithFactoryWithParams();
    }
    
    [Benchmark]
    public PersonCreated3 CreateWithFactoryWithData()
    {
        return _request.CreateWithFactoryWithData();
    }
    
    [Benchmark]
    public PersonCreated4 CreateWithLegacyEnricher()
    {
        return _request.CreateWithLegacyEnricher(_enricher);
    }
    
    // [Benchmark]
    public List<PersonCreated1> ConstructorWithParams_List()
    {
        return _requests.Select(r =>r.CreateWithConstructorWithParams()).ToList();
    }
    
    // [Benchmark]
    public List<PersonCreated2> CreateWithFactoryWithParams_List()
    {
        return _requests.Select(r =>r.CreateWithFactoryWithParams()).ToList();
    }
    
    [Benchmark]
    public List<PersonCreated3> CreateWithFactoryWithData_List()
    {
        return _requests.Select(r =>r.CreateWithFactoryWithData()).ToList();
    }
    
    [Benchmark]
    public List<PersonCreated4> CreateWithLegacyEnricher_List()
    {
        return _requests.Select(r =>r.CreateWithLegacyEnricher(_enricher)).ToList();
    }
}

