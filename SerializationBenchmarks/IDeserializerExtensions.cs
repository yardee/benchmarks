using BTDB.StreamLayer;

namespace SerializationBenchmarks;

public static class DeserializerExtensions
{
    public static unsafe object? Deserialize(this IDeserializer deserializer, byte[] data)
    {
        fixed(void * _ = data)
        {
            var reader = MemReader.CreateFromPinnedSpan(data.AsSpan());
            var obj = deserializer.Deserialize(ref reader);
            return obj;
        }
    }
}