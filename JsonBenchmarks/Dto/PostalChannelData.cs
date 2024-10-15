namespace JsonBenchmarks.Dto;

public record PostalChannelData
{
    public string? DeliveryServiceName { get; set; }
    public string? DeliveryServiceIdentifier { get; set; }
    public string? Envelope { get; set; }
    public string? EnvelopeName { get; set; }
    public bool? ProofOfMailingInfoRequested { get; set; }
    public bool? ProofOfMailingInfoAvailable { get; set; }
    public bool? PrintedInfoAvailable { get; set; }
    public bool? EnvelopedInfoAvailable { get; set; }
    public bool? PostedInfoAvailable { get; set; }
    public bool? TransmissionInfoAvailable { get; set; }
    public bool? Stapling { get; set; }
    public IList<SubsetStaplingRange>? SubsetStaplingRanges { get; set; }
    public bool? ReturnEnvelope { get; set; }
    public bool? Color { get; set; }
    public bool? Duplex { get; set; }
    public string? Sheet { get; set; }
    public uint? PageCount { get; set; }
    public uint? SheetCount { get; set; }
    public bool? InternationalMail { get; set; }
    public uint? WeightGrams { get; set; }
    public TransmissionInfo? TransmissionInfo { get; set; }
    public PrintedInfo? PrintedInfo { get; set; }
    public EnvelopedInfo? EnvelopedInfo { get; set; }
    public PostedInfo? PostedInfo { get; set; }
    public IList<CustomBoolOption>? CustomBoolOptions { get; set; }
    public IList<CustomListOption>? CustomListOptions { get; set; }
    public string? ReturnAddress { get; set; }
    public string? ReplyAddress { get; set; }
    public string? SenderAddress { get; set; }
    public bool? SenderAndReturnAddressSeparated { get; set; }
    public bool? UseReturnAddressOnReturnEnvelope { get; set; }
    public bool? ChangedProductionConfiguration { get; set; }
    public bool? PostedSubServiceInfoAvailable { get; set; }
    public bool? DeliverySubServiceInfoAvailable { get; set; }
    public bool? DeliverySuccessSubServiceInfoAvailable { get; set; }
    public bool? DeliveryFailSubServiceInfoAvailable { get; set; }
    public IList<SubService>? RequestedSubServices { get; set; }
    public IList<SubService>? ConfirmedSubServices { get; set; }
    public IList<SubServiceInfo>? SubServiceInfos { get; set; }
    public IList<IncidentInfo>? IncidentInfos { get; set; }
    public bool? UseAddressCarrier { get; set; }

    [Obsolete("Since 1.3")]
    public bool? UndeliveredNotificationInfoRequested { get; set; }

    [Obsolete("Since 1.3")]
    public bool? UndeliveredNotificationInfoAvailable { get; set; }

    [Obsolete("Since 1.3")]
    public bool? TrackingInfoRequested { get; set; }

    [Obsolete("Since 1.3")]
    public string? TrackingInfoUrl { get; set; }

    [Obsolete("Since 1.3")]
    public bool? TrackingInfoAvailable { get; set; }

    [Obsolete("Since 1.3")]
    public bool? ProofOfDeliveryInfoRequested { get; set; }

    [Obsolete("Since 1.3")]
    public bool? ProofOfDeliveryInfoAvailable { get; set; }

    [Obsolete("Since 1.3")]
    public TrackingInfo? TrackingInfo { get; set; }

    [Obsolete("Since 1.3")]
    public ProofOfDeliveryInfo? ProofOfDeliveryInfo { get; set; }

    [Obsolete("Since 1.3")]
    public UndeliveredNotificationInfo? UndeliveredNotificationInfo { get; set; }
}
