namespace JsonBenchmarks.Dto;

public record CommunicationPieceInvoiceMetadata 
{
    /// <summary>
    /// Base communication piece metadata for archive Id and expiration date
    /// </summary>
    public CommunicationPieceMetadataInfo? CommunicationPieceInfo { get; set; }

    public string? ArchiveId { get; set; }
    public ulong? TeamId { get; set; }
    public string? Channel { get; set; }
    public string? JobCategoryIdentifier { get; set; }

    /// <summary>
    /// Source for create the parent invoice metadata object
    /// </summary>
    public InvoiceInfo? InvoiceInfo { get; set; }

    public string? CustomerClientId { get; set; }
    public string? JobOrigin { get; set; }
}
