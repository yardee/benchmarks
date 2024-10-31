using BTDB.EventStoreLayer;
using BTDB.StreamLayer;

namespace SerializationBenchmarks;

public class Serializer
{
    internal const byte TypeDescriptor = 99;
    internal const byte Nothing = 100;

    readonly ITypeSerializersMapping _mapping;

    public Serializer(ITypeSerializerMappingFactory typeSerializerMappingFactory)
    {
        _mapping = typeSerializerMappingFactory.CreateMapping();
    }

    public Serializer(ITypeSerializersMapping mapping)
    {
        _mapping = mapping;
    }

    internal void Serialize(ref MemWriter writer, object @object)
    {
        var start = writer.GetCurrentPosition();

        writer.WriteUInt8(Nothing);
        var serializerContext = _mapping.StoreNewDescriptors(@object);
        serializerContext.FinishNewDescriptors(ref writer);
        serializerContext.StoreObject(ref writer, @object);
        if (serializerContext.SomeTypeStored)
        {
            var end = writer.GetCurrentPosition();
            writer.SetCurrentPosition(start);
            writer.WriteUInt8(TypeDescriptor);
            writer.SetCurrentPosition(end);
        }

        serializerContext.CommitNewDescriptors();
    }

    public byte[] Serialize(object @object)
    {
        var writer = new MemWriter();
        Serialize(ref writer, @object);
        var data = writer.GetSpan().ToArray();
        return data;
    }
}