namespace JsonBenchmarks.Dto;

public record ClientPortalChannelData
{
    public bool? NotifyClientWhenDelivered { get; set; }
    public bool? Read { get; set; }
    public DateTime? FinishedDateTime { get; set; }
    public DateTime? ProcessingFailedDateTime { get; set; }
    public DateTime? ReadDateTime { get; set; }
    public uint? RetentionPeriodInYears { get; set; }
}
