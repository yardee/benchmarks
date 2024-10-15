namespace JsonBenchmarks.Dto;

public record EnvelopedInfo
{
    public DateTime? Date { get; set; }
    public string? InserterName { get; set; }
    public string? EnvelopeType { get; set; }
    public bool? IsFailed { get; set; }
    public string? ServiceProviderSystemGroupId { get; set; }
    public string? ServiceProviderSystemEntityId { get; set; }
}
