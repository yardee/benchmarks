#nullable enable

namespace JsonBenchmarks.Dto;

public class TransactionInfo
{
    public string? TransactionId { get; set; }
    public string? AuthCode { get; set; }
    public int? PaymentId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountAccessory { get; set; }
    public string? AvsStatus { get; set; }
    public string? Processor { get; set; }
    public string? CvvStatus { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? TransactionResult { get; set; }
}
