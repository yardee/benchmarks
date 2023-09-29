using System.Diagnostics;
using BenchmarkDotNet.Attributes;

#nullable enable

namespace LambdaMemory;

[MemoryDiagnoser]
public class CompareLambdaAllocations
{
    private const int InitCount = 100000;
    private const int WaitForCount = 101000;
    private const int WaitMs = 1;

    [Benchmark]
    public async Task<bool> VariableInsideLambda()
    {
        var count = InitCount;
        var res = await Awaiter(() =>
        {
            var persons = GetResponse(++count).ToList();
            Console.WriteLine($"Count: {persons.Count}");
            return Task.FromResult(persons);
        }, p => p.Count >= WaitForCount);

        return res;
    }

    [Benchmark]
    public async Task<bool> VariableAwaiterInstance()
    {
        var count = InitCount;
        var awaited = new Awaited(count);
        var res = await Awaiter(awaited);
        return res;
    }

    static IEnumerable<Person> GetResponse(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new Person
            {
                Name =
                    $"Name asdf asdf adf asdf sadf asd f sdaf sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asd sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asdf s{i}",

                AddressLine1 =
                    $"Address asdf asdf adf asdf sadf asd f sdaf sdafsdaf sdaf sdaffffffff asdfasd fasdf asdf asdf asdf asdf asdf asdf asdf asdf asdf asdf s{i}",
                AddressLine3 =
                    $"Address asdf asdfasdf asd fasdf asd fasdf asdf asdf asd fasdf 456adf 456asfd 65asdf65asdf 56asd56as d56asdf56asdf56asdf 65asd465asdf65asdf6as5df6a5sf465asdf56asdf65asd465as4d 6as4df 6 5as465asdf4as65df4a6s5df a6s5df6a5sdf 6a5sd4 6a5sdfa6s5df 6a5sdf as65df a5s6df as65d 4a56sdf56 asdf 56asdf 56asd4 65asd4 65asdf 56asdfa65sdf4a6s5df4 as65df4as65d 4as65d4 as65d46as5df46as5df6safd sda {i}",
                AddressLine2 =
                    $"Address asdf asdfasdf asd fasdf a4654645645654564654 sd fasdf asdf asdf asd fasdf 456adf 456asfd 65asdf65asdf 56asd56as d56asdf56asdf56asdf 65asd465asdf65asdf6as5df6a5sf465asdf56asdf65asd465as4d 6as4df 6 5as465asdf4as65df4a6s5df a6s5df6a5sdf 6a5sd4 6a5sdfa6s5df 6a5sdf as65df a5s6df as65d 4a56sdf56 asdf 56asdf 56asd4 65asd4 65asdf 56asdfa65sdf4a6s5df4 as65df4as65d 4as65d4 as65d46as5df46as5df6safd sda {i}",
            };
        }
    }

    async Task<bool> Awaiter<T>(Func<Task<T>> callback, Func<T, bool> condition)
    {
        var sw = Stopwatch.StartNew();

        while (true)
        {
            var res = await callback();
            if (condition(res))
            {
                break;
            }

            if (sw.Elapsed > TimeSpan.FromMinutes(5))
            {
                return false;
            }

            await Task.Delay(WaitMs);
        }

        return true;
    }

    async Task<bool> Awaiter<T>(IAwaited<T> awaited)
    {
        var sw = Stopwatch.StartNew();

        while (true)
        {
            var res = await awaited.Do();
            if (awaited.Condition(res))
            {
                break;
            }

            if (sw.Elapsed > TimeSpan.FromMinutes(5))
            {
                return false;
            }

            await Task.Delay(WaitMs);
        }

        return true;
    }

    public interface IAwaited<T>
    {
        Task<T> Do();
        Func<T, bool> Condition { get; }
    }

    private class Awaited : IAwaited<List<Person>>
    {
        private int _count;

        public Awaited(int count)
        {
            _count = count;
        }

        public Task<List<Person>> Do()
        {
            var persons = GetResponse(++_count).ToList();
            Console.WriteLine($"Count: {persons.Count}");
            return Task.FromResult(persons);
        }

        public Func<List<Person>, bool> Condition => p => p.Count >= WaitForCount;
    }
}