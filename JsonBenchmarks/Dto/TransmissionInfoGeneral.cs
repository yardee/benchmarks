namespace JsonBenchmarks.Dto;

public record TransmissionInfoGeneral
{
    public DateTime? Date { get; set; }
    public bool? IsFailed { get; set; }
}
