namespace JsonBenchmarks.Dto;

public record UnsubscribedInfo
{
    public DateTime? Date { get; set; }
    public string? Reason { get; set; }
}
