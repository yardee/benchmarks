
namespace JsonBenchmarks.Dto;

public record SubServiceInfo 
{
    public DateTime? Date { get; set; }
    public int? Phase { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? StringValue { get; set; }
    public string? UrlValue { get; set; }
    public ulong? DocumentId { get; set; }
    public string? ProviderName { get; set; }
    public string? ProviderServiceName { get; set; }
    public string? ProviderSystemEntityId { get; set; }
    public string? ProviderSystemGroupId { get; set; }
    public string? Note { get; set; }
}
