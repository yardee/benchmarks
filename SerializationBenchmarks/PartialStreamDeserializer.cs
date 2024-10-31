using BTDB;
using BTDB.EventStoreLayer;
using BTDB.StreamLayer;

namespace SerializationBenchmarks;

[Generate]
public class PartialStreamDeserializer : IDeserializer
{
    readonly Deserializer _deserializer;

    public PartialStreamDeserializer(ITypeSerializerMappingFactory typeSerializerMappingFactory)
    {
        _deserializer = new Deserializer(typeSerializerMappingFactory);
    }

    public object? Deserialize(ref MemReader reader)
    {
        int length = -1;
        string? name = null;
        long start = -1, end = -1;
        object? obj = null;

        try
        {
            length = reader.ReadInt32BE();
            name = reader.ReadString();

            start = reader.GetCurrentPosition();
            obj = _deserializer.Deserialize(ref reader);
            end = reader.GetCurrentPosition();

            var size = (int)(end - start);
            if (length != size)
                throw new InvalidDataException($"Signature does not match, expected object with length {length} but got {size}");
            return obj;
        }
        catch (Exception)
        {
            throw;
        }
    }
}