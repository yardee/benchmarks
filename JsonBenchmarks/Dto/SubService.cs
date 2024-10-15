namespace JsonBenchmarks.Dto;

public record SubService
{
    public int? Phase { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public bool? VoidEvidence { get; set; }
    public bool? StringEvidence { get; set; }
    public bool? UrlEvidence { get; set; }
    public bool? FileEvidence { get; set; }
    public bool? Optional { get; set; }
    public bool? Repeatable { get; set; }
}
