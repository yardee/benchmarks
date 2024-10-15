#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Gmc.Cloud.Infrastructure.Utils;
using JsonBenchmarks.Dto;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

static class MetadataGeneratorUtils
{
    public static bool IsList(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);

    public static object? GetMinMaxValues(GenerationMode generationMode, Type type, string[] propertyPath)
    {
        var valueName = propertyPath[^1];

        return type switch
        {
            _ when type == typeof(bool) => true,
            _ when type == typeof(int) => generationMode == GenerationMode.MinValues ? int.MinValue : int.MaxValue,
            _ when type == typeof(uint) => generationMode == GenerationMode.MinValues ? uint.MinValue : uint.MaxValue,
            _ when type == typeof(long) => generationMode == GenerationMode.MinValues ? long.MinValue : long.MaxValue,
            _ when type == typeof(ulong) => generationMode == GenerationMode.MinValues ? ulong.MinValue : ulong.MaxValue,
            _ when type == typeof(decimal) => generationMode == GenerationMode.MinValues ? decimal.MinValue : decimal.MaxValue,
            _ when type == typeof(double) => generationMode == GenerationMode.MinValues ? double.MinValue : double.MaxValue,
            _ when type == typeof(DateTime) => new DateTime(2019, 12, 24, 17, 42, 59, DateTimeKind.Utc)
                .AddDays(generationMode == GenerationMode.MinValues ? 0 : 100),
            _ when type == typeof(string) => valueName + " " + (generationMode == GenerationMode.MinValues ? "x" : string.Join("", (Enumerable.Repeat(0, 100).Select(_ => "x")))),
            _ when IsList(type) => generationMode == GenerationMode.MinValues ? 1 : 10,
            _ when type == typeof(Guid) => Guid.NewGuid(),
            _ when type.IsClass && type != typeof(string) => type.CreatePristineObject<object>(),
            _ => throw new NotSupportedException(
                $"Cannot generate data for unsupported type {type} of value '{valueName}'. " +
                $"If this is a new value, add a generator for its type."),
        };
    }

    public static string[][] GetCommunicationPiecePropertyPathsToSkip() =>
    [
        [nameof(Metadata.InvoiceMetadataParts)]
    ];

    public static string[][] GetInvoicePropertyPathsToSkip() =>
    [
        [ nameof(Metadata.Variables) ],
        [ nameof(Metadata.Approvers) ],
        [ nameof(Metadata.PostalChannelData) ],
        [ nameof(Metadata.EmailChannelData) ],
        [ nameof(Metadata.SmsChannelData) ],
        [ nameof(Metadata.ArchiveChannelData) ],
        [ nameof(Metadata.ClientPortalChannelData) ],
        [ nameof(Metadata.EreSimpleChannelData) ],
        [ nameof(Metadata.EInvoiceChannelData) ],
        [ nameof(Metadata.CommunicationPreparationChannelData) ],
        [ nameof(Metadata.FaxChannelData) ],
        [ nameof(Metadata.PreprocessingPreparationInfo) ],
        [ nameof(Metadata.SentToProductionInfo) ],
        [ nameof(Metadata.TransmissionInfo) ],
        [ nameof(Metadata.SigningInfo) ],
        [ nameof(Metadata.ExternalAttributes) ],
        [ nameof(Metadata.DeliveryStatusInfo) ],
        [ nameof(Metadata.RecipientData) ],
    ];

    public static string[][] GetMetadataPropertyPathsToSkip() =>
    [
        [nameof(Metadata.InvoiceInfo), nameof(InvoiceInfo.InvoiceCalculatedAmounts)],
        [nameof(Metadata.InvoiceMetadataParts), nameof(CommunicationPieceInvoiceMetadata.InvoiceInfo), nameof(InvoiceInfo.InvoiceCalculatedAmounts)],
    ];
}
