namespace JsonBenchmarks.Dto;

public record TransmissionInfo 
{
    public DateTime? Date { get; set; }
    public string? ServiceProviderSystemEntityId { get; set; }
    public string? ServiceProviderSystemGroupId { get; set; }
    public bool? IsFailed { get; set; }
}
