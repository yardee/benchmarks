namespace JsonBenchmarks.Dto;

public interface IElectronicChannelData
{
    bool? DeliveredInfoAvailable { get; set; }
    bool? UndeliveredInfoAvailable { get; set; }
    bool? UnsubscribedInfoAvailable { get; set; }
    bool? StopTrackingInfoAvailable { get; set; }
    ulong? ServiceProviderId { get; set; }
    Guid? ServiceProviderIdentifier { get; set; }
    SentInfo? SentInfo { get; set; }
    DeliveredInfo? DeliveredInfo { get; set; }
    UndeliveredInfo? UndeliveredInfo { get; set; }
    UnsubscribedInfo? UnsubscribedInfo { get; set; }
    StopTrackingInfo? StopTrackingInfo { get; set; }
}
