#nullable enable

using BenchmarkDotNet.Attributes;

namespace CollectionBenchmarks;

[MemoryDiagnoser]
public class CompareCollections
{
    // private const int Count = 1000;
    // private Queue<int> _queue;
    // private List<int> _list;
    
    [Benchmark]
    public ulong ValueUlong()
    {
        ulong a = 100;
        return Do(a);
    }
    
    [Benchmark]
    public ulong? ValueNullableUlong()
    {
        ulong? a = 100;
        return Do(a);
    }
    
    [Benchmark]
    public ulong ValueZeroUlong()
    {
        ulong a = 0;
        return Do(a);
    }
    
    [Benchmark]
    public ulong? ValueNullUlong()
    {
        ulong? a = null;
        return Do(a);
    }

    private T Do<T>(T a)
    {
        return a;
    }
    
    
    /*[GlobalSetup]
    public void GlobalSetup()
    {
        _queue = new Queue<int>();
        _list = new List<int>();
        foreach (var i in Enumerable.Range(0, Count))
        {
            _queue.Enqueue(i);
            _list.Add(i);
        }
    }
    
    
    [Benchmark]
    public void Queue_Enqueue()
    {
        var queue = new Queue<int>();
        foreach (var i in Enumerable.Range(0, Count))
        {
            queue.Enqueue(i);
        }
    }
    
    [Benchmark]
    public void List_Add()
    {
        var list = new List<int>();
        foreach (var i in Enumerable.Range(0, Count))
        {
            list.Add(i);
        }
    }
    
    [Benchmark]
    public void List_Remove()
    {
        _list.Remove(500);
    }
    
    [Benchmark]
    public void Queue_Remove()
    {
        _queue = new Queue<int>(_queue.Where(e => e != 500));
    }
    
    [Benchmark]
    public void StackallocSpan_Create()
    {
        Span<int> span = stackalloc int[Count];

        foreach (var i in Enumerable.Range(0, Count))
        {
            if (i != 5)
            {
                span[i] = i;
            }
        }
    }
    
    [Benchmark]
    public void ArraySpan_Create()
    {
        var span = new Span<int>(new int[Count]);

        foreach (var i in Enumerable.Range(0, Count))
        {
            if (i != 5)
            {
                span[i] = i;
            }
        }
    }
    
    [Benchmark]
    public void Array_Add()
    {
        var array = new int[Count];
        foreach (var i in Enumerable.Range(0, Count))
        {
            if (i != 5)
            {
                array[i] = i;
            }
        }
    }
    
    [Benchmark]
    public object HashSet_FromIEnumerable()
    {
        var array = new HashSet<int>(Enumerable.Range(0, Count));
        return array;
    }
    
    [Benchmark]
    public Span<int> Span_FromIEnumerable()
    {
        return new Span<int>(Enumerable.Range(0, Count).ToArray());
    }*/
    
}