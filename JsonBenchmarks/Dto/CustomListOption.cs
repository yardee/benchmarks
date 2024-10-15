namespace JsonBenchmarks.Dto;

public record CustomListOption
{
    public string? Name { get; set; }
    public string? Identifier { get; set; }
    public string? Value { get; set; }
    public string? ValueIdentifier { get; set; }
    public string? Type { get; set; }
}
