namespace JsonBenchmarks.Dto;

public record SmsChannelData : IElectronicChannelData
{
    public bool? DeliveredInfoRequested { get; set; }
    public bool? DeliveredInfoAvailable { get; set; }
    public bool? UndeliveredInfoAvailable { get; set; }
    public bool? UnsubscribedInfoAvailable { get; set; }
    public bool? StopTrackingInfoAvailable { get; set; }
    public uint? MessagePartsCount { get; set; }
    public ulong? ServiceProviderId { get; set; }
    public Guid? ServiceProviderIdentifier { get; set; }
    public SentInfo? SentInfo { get; set; }
    public DeliveredInfo? DeliveredInfo { get; set; }
    public UndeliveredInfo? UndeliveredInfo { get; set; }
    public UnsubscribedInfo? UnsubscribedInfo { get; set; }
    public StopTrackingInfo? StopTrackingInfo { get; set; }
}
