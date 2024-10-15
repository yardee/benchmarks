namespace JsonBenchmarks.Dto;

public record EmailChannelData : IElectronicChannelData
{
    public bool? DeliveredInfoAvailable { get; set; }
    public bool? DeliveryReceiptInfoRequested { get; set; }
    public bool? ReadReceiptInfoRequested { get; set; }
    public bool? OpenInfoAvailable { get; set; }
    public bool? UndeliveredInfoAvailable { get; set; }
    public bool? UnsubscribedInfoAvailable { get; set; }
    public bool? SpamInfoAvailable { get; set; }
    public bool? StopTrackingInfoAvailable { get; set; }
    public DeliveredInfo? DeliveredInfo { get; set; }
    public OpenedInfo? OpenedInfo { get; set; }
    public FirstOpenedInfo? FirstOpenedInfo { get; set; }
    public UndeliveredInfo? UndeliveredInfo { get; set; }
    public UnsubscribedInfo? UnsubscribedInfo { get; set; }
    public SentInfo? SentInfo { get; set; }
    public SpamInfo? SpamInfo { get; set; }
    public StopTrackingInfo? StopTrackingInfo { get; set; }
    public string? ReplyEmail { get; set; }
    public string? ReplyName { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
    public ulong? ServiceProviderId { get; set; }
    public Guid? ServiceProviderIdentifier { get; set; }
}
