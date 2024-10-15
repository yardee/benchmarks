#nullable enable

using System;
using System.Linq;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

class CommunicationPieceMetadataGeneratorMinMaxValuesStrategy(GenerationMode generationMode) : IMetadataGeneratorStrategy
{
    public bool ShouldSkipProperty(Type type, string[] propertyPath) =>
        MetadataGeneratorUtils.GetCommunicationPiecePropertyPathsToSkip()
            .Any(propertyPathToSkip => propertyPathToSkip.SequenceEqual(propertyPath));

    public object? GetValue(Type type, string[] propertyPath) =>
        MetadataGeneratorUtils.GetMinMaxValues(generationMode, type, propertyPath);
}
