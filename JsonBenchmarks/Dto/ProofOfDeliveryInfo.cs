namespace JsonBenchmarks.Dto;

public record ProofOfDeliveryInfo
{
    public DateTime? Date { get; set; }
    public ulong? ProofByDocumentId { get; set; }
}
