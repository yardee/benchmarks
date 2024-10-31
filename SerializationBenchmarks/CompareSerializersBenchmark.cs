using System.IO.Compression;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BTDB.EventStoreLayer;
using BTDB.ODBLayer;
using Newtonsoft.Json;

namespace SerializationBenchmarks;

interface IDynamicValue
{
    object? InternalValue { get; set; }
}
interface IDynamicValue<TValueType>: IDynamicValue
{
    TValueType Value { get; set; }
    
    [NotStored]
    object? IDynamicValue.InternalValue
    {
        get => Value;
        set
        {
            if (value is TValueType typedValue)
            {
                Value = typedValue;
            }
            else if (value == null && default(TValueType) == null)
            {
                Value = default!;
            }
            else
            {
                var valueType = value == null ? "null" : value.GetType().ToString();
                throw new InvalidCastException($"Cant cast {valueType} to {typeof(TValueType)}");
            }
        }
    }
}

public class Money
{
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
}

public class DynamicValueWrapper<TValueType>(TValueType value) : IDynamicValue<TValueType>
{
    public TValueType Value { get; set; } = value;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((DynamicValueWrapper<TValueType>)obj);
    }

    public override int GetHashCode()
    {
        return Value != null ? EqualityComparer<TValueType>.Default.GetHashCode(Value) : 0;
    }
}

[MemoryDiagnoser]
public class CompareSerializersBenchmark
{
    private IDynamicValue _value = default!;
    private JsonSerializerSettings _serializerSettings = default!;
    private string _json = default!;
    private string _jsonCompressed = default!;
    private string _serializerBase64 = default!;
    private string _serializerCompressedBase64 = default!;
    private Serializer Serializer => new Serializer(new TypeSerializers());
    private Deserializer Deserializer => new Deserializer(new TypeSerializers());
    [GlobalSetup]
    public void GlobalSetup()
    {
        _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
        // fill the dictionary with huge number of values
        _value = new DynamicValueWrapper<Dictionary<ulong, IDynamicValue>>(Enumerable.Range(1, 100).ToDictionary(
            i => (ulong)i, IDynamicValue (i) => new DynamicValueWrapper<Dictionary<ulong, IDynamicValue>>(new Dictionary<ulong, IDynamicValue>
            {
                { 1, new DynamicValueWrapper<string>("value") },
                { 2, new DynamicValueWrapper<Money>(new Money
                {
                    Amount = 10500,
                    Currency = "USD"
                }) },
            })
        ));

        _json = JSON_Serialize();
        _jsonCompressed= JSON_Compressed_Serialize();
        _serializerBase64 = Serializer_Serialize();
        _serializerCompressedBase64 = Serializer_Compressed_Serialize();
        
    }
    
    [Benchmark]
    public string JSON_Serialize()
    {
        return JsonConvert.SerializeObject(_value.InternalValue, _serializerSettings);
    }
    
    [Benchmark]
    public string JSON_Compressed_Serialize()
    {
        var json = JsonConvert.SerializeObject(_value.InternalValue, _serializerSettings);
        var bytes = Encoding.UTF8.GetBytes(json);
        using var memoryStream = new MemoryStream();
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
        gzipStream.Write(bytes, 0, bytes.Length);
        gzipStream.Close();
        return Convert.ToBase64String(memoryStream.ToArray());
    }
    
    [Benchmark]
    public string Serializer_Serialize()
    {
        var bytes = Serializer.Serialize(_value.InternalValue!);
        return Convert.ToBase64String(bytes);
    }
    
    [Benchmark]
    public object JSON_Deserialize()
    {
        return JsonConvert.DeserializeObject(_json, _value.InternalValue!.GetType(), _serializerSettings)!;
    }
    
    [Benchmark]
    public object JSON_Compressed_Deserialize()
    {
        var byteArray = Convert.FromBase64String(_jsonCompressed);
        using var memoryStream = new MemoryStream(byteArray);
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
        using var outputStream = new MemoryStream();
        gzipStream.CopyTo(outputStream);
        
        return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(outputStream.ToArray()), _value.InternalValue!.GetType(), _serializerSettings)!;
    }
    
    [Benchmark]
    public object Serializer_Deserialize()
    {
        var bytes = Convert.FromBase64String(_serializerBase64);
        return Deserializer.Deserialize(bytes)!;

    }
    
    [Benchmark]
    public string Serializer_Compressed_Serialize()
    {
        var bytes = Serializer.Serialize(_value.InternalValue!);
        using var memoryStream = new MemoryStream();
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
        gzipStream.Write(bytes, 0, bytes.Length);
        gzipStream.Close();
        var compressedBytes = memoryStream.ToArray();
        return Convert.ToBase64String(compressedBytes);
    }
    
    [Benchmark]
    public object Serializer_Compressed_Deserialize()
    {
        var compressedBytes = Convert.FromBase64String(_serializerCompressedBase64);
        using var inputStream = new MemoryStream(compressedBytes);
        using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
        using var outputStream = new MemoryStream();
        gzipStream.CopyTo(outputStream);
        var decompressedBytes = outputStream.ToArray();
        
        return Deserializer.Deserialize(decompressedBytes)!;
    }
}
