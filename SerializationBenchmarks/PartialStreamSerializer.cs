using BTDB;
using BTDB.EventStoreLayer;
using BTDB.StreamLayer;

namespace SerializationBenchmarks;

[Generate]
public class PartialStreamSerializer(ITypeSerializerMappingFactory typeSerializerMappingFactory)
{
    readonly Serializer _serializer = new Serializer(typeSerializerMappingFactory);

    public byte[] Serialize(object @object)
    {
        string? name = null;
        int size = -1;

        try
        {
            name = @object.GetType().FullName;
            var writer = new MemWriter();
            writer.WriteInt32BE(0);
            writer.WriteString(name);

            var start = writer.GetCurrentPosition();
            _serializer.Serialize(ref writer, @object);
            var end = writer.GetCurrentPosition();

            size = (int)(end - start);
            writer.SetCurrentPosition(0);
            writer.WriteInt32BE(size);
            writer.SetCurrentPosition(end);

            var data = writer.GetSpan().ToArray();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}