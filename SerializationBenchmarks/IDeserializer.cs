using System;
using System.IO;
using BTDB.Buffer;
using BTDB.StreamLayer;

namespace SerializationBenchmarks;

public interface IDeserializer
{
    object? Deserialize(ref MemReader reader);
}