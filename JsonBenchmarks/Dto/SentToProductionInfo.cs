namespace JsonBenchmarks.Dto;

public record SentToProductionInfo
{
    public DateTime? Date { get; set; }
    public bool? IsFailed { get; set; }
}
