#nullable enable

using System;
using System.Linq;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

class InvoiceMetadataGeneratorMinMaxValuesStrategy(GenerationMode generationMode) : IMetadataGeneratorStrategy
{
    public bool ShouldSkipProperty(Type type, string[] propertyPath) =>
        MetadataGeneratorUtils.GetInvoicePropertyPathsToSkip()
            .Any(propertyPathToSkip => propertyPathToSkip.SequenceEqual(propertyPath));

    public object? GetValue(Type type, string[] propertyPath) =>
        MetadataGeneratorUtils.GetMinMaxValues(generationMode, type, propertyPath);
}
