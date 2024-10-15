#nullable enable
namespace JsonBenchmarks.Dto;

// NOTE: Any changes to the structure must be reflected in the LocalMetadata, DbMetadata and SearchParams classes. (and SearchFilterBuilder + MetadataIndex)
// Used to store metadata in ImpressX Archive
public partial class Metadata : ICloneable
{
    // NOTE: this class should NOT initialize any members to default values as this would interfere with metadata (diff) patching
    public ulong? JobId { get; set; }

    /// <summary>
    /// Either CommunicationPieceId or InvoiceId
    /// </summary>
    public ulong? ObjectId { get; set; }

    public ulong? CreatedByUserId { get; set; }
    public uint? ArchiveDurationInMonths { get; set; }
    public string? JobOrigin { get; set; }
    public string? SenderEmail { get; set; }
    public string? Channel { get; set; }
    public string? MessageType { get; set; }
    public string? ObjectType { get; set; }
    public string? Subject { get; set; }
    public string? JobName { get; set; }
    public string? RecipientName { get; set; }
    public string? RecipientContact { get; set; }
    public DateTime? ApprovedForProductionDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? Status { get; set; }
    public string? RecipientCountry { get; set; }
    public string? CostCenter { get; set; }
    public ulong? RecipientId { get; set; }
    public string? CustomerClientId { get; set; }
    public string? BucketOfService { get; set; }
    public string? BucketOfServiceCountry { get; set; }
    public ulong? LocalOfficeId { get; set; }
    public string? CustomCommunicationReference { get; set; }
    public string? CommunicationLanguage { get; set; }
    public IList<UserAttribute>? UserAttributes { get; set; }
    public IList<Variable>? Variables { get; set; }
    public IList<string>? Approvers { get; set; }
    public ProductionFailedInfo? ProductionFailedInfo { get; set; }
    public PostalChannelData? PostalChannelData { get; set; }
    public EmailChannelData? EmailChannelData { get; set; }
    public SmsChannelData? SmsChannelData { get; set; }
    public ArchiveChannelData? ArchiveChannelData { get; set; }
    public ClientPortalChannelData? ClientPortalChannelData { get; set; }
    public EreSimpleChannelData? EreSimpleChannelData { get; set; }
    public EInvoiceChannelData? EInvoiceChannelData { get; set; }
    public CommunicationPreparationChannelData? CommunicationPreparationChannelData { get; set; }
    public FaxChannelData? FaxChannelData { get; set; }
    public PreprocessingPreparationInfo? PreprocessingPreparationInfo { get; set; }
    public SentToProductionInfo? SentToProductionInfo { get; set; }
    public TransmissionInfoGeneral? TransmissionInfo { get; set; }
    public uint? ConfirmedReceptionState { get; set; }
    public Guid? DeliveryServiceIdentifier { get; set; }
    public int? MetadataVersion { get; set; }

    [Obsolete("From version 1.2.2")]
    public ulong? ServiceProviderId { get; set; }

    public Guid? ServiceProviderIdentifier { get; set; }
    public uint? TransactionCount { get; set; }
    public ulong? ParentJobId { get; set; }
    public ulong? TeamId { get; set; }
    public string? ExternalJobId { get; set; }
    public string? ExternalCommunicationId { get; set; }
    public string? JobCategoryIdentifier { get; set; }
    public int? CommunicationSource { get; set; }

    [Obsolete("use document in Archive")]
    public string? QiifBlobName { get; set; }

    public bool? HasDigitalAttachments { get; set; }
    public InvoiceInfo? InvoiceInfo { get; set; }
    public SigningInfo? SigningInfo { get; set; }
    public ExternalAttributes? ExternalAttributes { get; set; }
    public DeliveryStatusInfo? DeliveryStatusInfo { get; set; }
    public RecipientData? RecipientData { get; set; }
    public IList<CommunicationPieceInvoiceMetadata>? InvoiceMetadataParts { get; set; }

    object ICloneable.Clone()
    {
        return MemberwiseClone();
    }
}
