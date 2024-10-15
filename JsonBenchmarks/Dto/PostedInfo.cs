namespace JsonBenchmarks.Dto;

public record PostedInfo 
{
    public DateTime? Date { get; set; }
    public decimal? PostageCost { get; set; }
    public decimal? FullCost { get; set; }
    public string? CostUnit { get; set; }
    public uint? WeightGrams { get; set; }
    public string? PostalProviderName { get; set; }
    public string? PostalServiceName { get; set; }
    public bool? InternationalMailSent { get; set; }

    [Obsolete("Since 1.3")]
    public bool? TrackingInfoUsed { get; set; }

    [Obsolete("Since 1.3")]
    public bool? ProofOfDeliveryInfoUsed { get; set; }

    [Obsolete("Since 1.3")]
    public bool? ProofOfMailingInfoUsed { get; set; }

    [Obsolete("Since 1.3")]
    public bool? UndeliveredNotificationInfoUsed { get; set; }

    public string? ConfirmedRecipientCountry { get; set; }
    public bool? IsFailed { get; set; }
    public string? ServiceProviderSystemGroupId { get; set; }
    public string? ServiceProviderSystemEntityId { get; set; }
    public string? ConfirmedServiceIdentifier { get; set; }
    public string? ConfirmedServiceName { get; set; }
}
