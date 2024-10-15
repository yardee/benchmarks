namespace JsonBenchmarks.Dto;

public record UndeliveredInfo
{
    public DateTime? Date { get; set; }
    public string? Reason { get; set; }
}
