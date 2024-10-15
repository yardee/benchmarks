namespace JsonBenchmarks.Dto;

public record SigningInfo
{
    public string? RequestedProviderName { get; set; }
    public string? RequestedProviderId { get; set; }
    public string? RequestedSignatureLevel { get; set; }
    public DateTime? SentToSigningDate { get; set; }
    public DateTime? SigningFinishedDate { get; set; }
    public bool? WasSigningSuccessful { get; set; }
}
