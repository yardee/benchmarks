namespace JsonBenchmarks.Dto;

public record CommunicationSubmissionEventData
{
    public ulong CommunicationPieceId { get; set; }
    public int Channel { get; set; }
    public ulong TeamId { get; set; }
    public string? JobCategoryIdentifier { get; set; }
    public Guid? DeliveryServiceIdentifier { get; set; }
}
