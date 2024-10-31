using BTDB;
using BTDB.EventStoreLayer;
using BTDB.StreamLayer;

namespace SerializationBenchmarks;

[Generate]
public class Deserializer : IDeserializer
{
    readonly ITypeSerializersMapping _mapping;

    public Deserializer(ITypeSerializerMappingFactory typeSerializerMappingFactory)
    {
        _mapping = typeSerializerMappingFactory.CreateMapping();
    }
    
    public Deserializer(ITypeSerializersMapping mapping)
    {
        _mapping = mapping;
    }

    public object? Deserialize(ref MemReader reader)
    {
        byte c0 = reader.ReadUInt8();
        if (c0 == Serializer.TypeDescriptor)
        {
            _mapping.LoadTypeDescriptors(ref reader);
        }
        else if (c0 != Serializer.Nothing)
        {
            throw new InvalidDataException("Data received from other side must Start with byte 99 or 100");
        }

        var obj = _mapping.LoadObject(ref reader);
        return obj;
    }
}