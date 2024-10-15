namespace JsonBenchmarks.Dto;

public record EreSimpleChannelData
{
    public string? Comment { get; set; }

    /// <summary>
    /// Obsolete, use DeliveryServiceIdentifier instead.
    /// </summary>
    public int RecipientQuality { get; set; }

    public Guid? DeliveryServiceIdentifier { get; set; }

    public int RecipientLocale { get; set; }
    public string? RecipientCompanyName { get; set; }
    public string? ExternalReferenceId { get; set; }
    public bool? AcceptCommunicationInfoAvailable { get; set; }
    public bool? DownloadInfoAvailable { get; set; }
    public bool? DownloadExpiredInfoAvailable { get; set; }
    public bool? ExpiredInfoAvailable { get; set; }
    public bool? RefusedInfoAvailable { get; set; }
    public bool? UndeliveredInfoAvailable { get; set; }
    public AcceptCommunicationInfo? AcceptCommunicationInfo { get; set; }
    public DownloadInfo? DownloadInfo { get; set; }
    public DownloadExpiredInfo? DownloadExpiredInfo { get; set; }
    public ExpiredInfo? ExpiredInfo { get; set; }
    public RefusedInfo? RefusedInfo { get; set; }
    public UndeliveredInfo? UndeliveredInfo { get; set; }
}
