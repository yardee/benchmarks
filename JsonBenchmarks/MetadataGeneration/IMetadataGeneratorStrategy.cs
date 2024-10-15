#nullable enable

using System;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

public delegate void VisitUsedValue(Type propertyType, string[] propertyPath, object? usedValue);

interface IMetadataGeneratorStrategy
{
    public bool ShouldSkipProperty(Type type, string[] propertyPath);
    public object? GetValue(Type type, string[] propertyPath);
}
