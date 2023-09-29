#nullable enable

using System.Text;
using BenchmarkDotNet.Attributes;

namespace StringBenchmarks;

[MemoryDiagnoser]
public class CompareStringCreation
{
    private Person[] _persons;
    private Logger _log;
    private TimeSpan _duration;
    static readonly TimeSpan SlowEventThreshold = TimeSpan.FromMilliseconds(1);
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _duration = TimeSpan.FromMilliseconds(20);
        _log = new Logger();
        _persons = new []
        {
            new Person { Id = 5, Name = "asdfasdffsda" },
            new Person { Id = 6, Name = "asdfasdffsd44545a" },
            new Person { Id = 7, Name = "asdfasdff455445455sda" },
        };
    }
    
    [Benchmark]
    public void String_WithVariables()
    {
        var message = "";

        for (var index = 0; index < _persons.Length; index++)
        {
            var person = _persons[index];
            var eventName = person.Name;
            var eventId = person.Id;
            message += $"{eventName}[{eventId}] - ";
            message += $"{_duration.TotalMilliseconds:F2} ms";
            if (_duration > SlowEventThreshold)
            {
                message += " (slow)";
            }
            
            if (index != _persons.Length - 1)
            {
                message += ", ";
            }
        }

        _log.Log($"Batch of events processed on: [{message}]");
    }
    
    [Benchmark]
    public void String_WithMinimumVariables()
    {
        var message = "";

        for (var index = 0; index < _persons.Length; index++)
        {
            var person = _persons[index];
            var eventName = person.Name;
            var eventId = person.Id;
            message += $"{eventName}[{eventId}] - {_duration.TotalMilliseconds:F2} ms {(_duration > SlowEventThreshold ? " (slow)" : "")}{(index != _persons.Length - 1 ? ", " : "")}";
        }

        _log.Log($"Batch of events processed on: [{message}]");
    }
    
    [Benchmark]
    public void StringBuilder_WithVariables()
    {
        var sb = new StringBuilder();

        for (var index = 0; index < _persons.Length; index++)
        {
            var person = _persons[index];
            var eventName = person.Name;
            var eventId = person.Id;
            sb.Append($"{eventName}[{eventId}] - ");
            sb.Append($"{_duration.TotalMilliseconds:F2} ms");

            if (_duration > SlowEventThreshold)
            {
                sb.Append(" (slow)");
            }

            if (index != _persons.Length - 1)
            {
                sb.Append(", ");
            }
        }

        _log.Log($"Batch of events processed on: [{sb.ToString()}]");
    }
    
    [Benchmark]
    public void StringBuilder_MinimumVariables()
    {
        var sb = new StringBuilder();

        for (var index = 0; index < _persons.Length; index++)
        {
            var person = _persons[index];
            var eventName = person.Name;
            var eventId = person.Id;
            sb.Append($"{eventName}[{eventId}] - {_duration.TotalMilliseconds:F2} ms {(_duration > SlowEventThreshold ? " (slow)" : "")}{(index != _persons.Length - 1 ? ", " : "")}");
        }

        _log.Log($"Batch of events processed on: [{sb.ToString()}]");
    }
}

class Person
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

class Logger
{
    public void Log(FormattableString message)
    {
        
    }
}