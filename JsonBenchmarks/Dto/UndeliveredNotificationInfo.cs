namespace JsonBenchmarks.Dto;

public record UndeliveredNotificationInfo
{
    public DateTime? Date { get; set; }
    public string? Reason { get; set; }
}
