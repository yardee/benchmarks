
namespace JsonBenchmarks.Dto;

public record PrintedInfo
{
    public DateTime? Date { get; set; }
    public string? PrinterName { get; set; }
    public string? PaperType { get; set; }
    public bool? IsFailed { get; set; }
    public string? ServiceProviderSystemGroupId { get; set; }
    public string? ServiceProviderSystemEntityId { get; set; }
}
