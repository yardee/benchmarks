namespace JsonBenchmarks.Dto;

public record CustomBoolOption
{
    public string? Name { get; set; }
    public string? Identifier { get; set; }
    public bool? Value { get; set; }
    public string? Type { get; set; }
}
