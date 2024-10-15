namespace JsonBenchmarks.Dto;

public record FaxChannelData
{
    public bool? CoverSheet { get; set; }
    public string? Comment { get; set; }
    public string? SenderFaxNumber { get; set; }
    public string? SenderName { get; set; }
    public string? Resolution { get; set; }
    public string? PageSize { get; set; }
    public string? ExternalSystemEntityId { get; set; }
    public IList<SubServiceInfo>? SubServiceInfos { get; set; }
    public IList<IncidentInfo>? IncidentInfos { get; set; }
    public bool? TrackingFinished { get; set; }
    public string? RecipientFaxNumber { get; set; }
}
