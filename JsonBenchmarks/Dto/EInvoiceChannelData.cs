namespace JsonBenchmarks.Dto;

public record EInvoiceChannelData
{

    public string? ExternalSystemEntityId { get; set; }
    public IList<SubServiceInfo>? SubServiceInfos { get; set; }
    public IList<IncidentInfo>? IncidentInfos { get; set; }
    public bool? TrackingFinished { get; set; }
}
