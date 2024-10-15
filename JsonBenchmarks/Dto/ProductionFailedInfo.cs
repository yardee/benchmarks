namespace JsonBenchmarks.Dto;

public record ProductionFailedInfo
{
    public DateTime? Date { get; set; }
    public string? Reason { get; set; }
}
