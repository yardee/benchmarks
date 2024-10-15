
namespace JsonBenchmarks.Dto;

public record IncidentInfo
{
    public DateTime? Date { get; set; }
    public uint? Code { get; set; }
    public string? Description { get; set; }
    public int? Severity { get; set; }
    public decimal? ExtraCost { get; set; }
    public string? CostUnit { get; set; }
    public string? Note { get; set; }
}
