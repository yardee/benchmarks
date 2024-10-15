namespace JsonBenchmarks.Dto;

public record CommunicationPieceMetadataInfo
{
    public ulong? CompanyId { get; set; }
    public ulong? CreatedByUserId { get; set; }
    public ulong? CommunicationPieceId { get; set; }
    public DateTime? CreationDate { get; set; }
    public uint? ArchiveDurationInMonths { get; set; }
    public DateTime? ArchiveExpirationDate { get; set; }

    public CommunicationPieceMetadataInfo() { }

    public CommunicationPieceMetadataInfo(CommunicationPieceInfo communicationPieceInfo)
    {
        CompanyId = communicationPieceInfo.CompanyId;
        CreatedByUserId = communicationPieceInfo.CreatedByUserId;
        CommunicationPieceId = communicationPieceInfo.CommunicationPieceId;
        CreationDate = communicationPieceInfo.CreationDate;
        ArchiveDurationInMonths = communicationPieceInfo.ArchiveDurationInMonths;
        ArchiveExpirationDate = communicationPieceInfo.ArchiveExpirationDate;
    }
}
