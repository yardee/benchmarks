namespace JsonBenchmarks.Dto;

public record InvoiceEvent
{
    public DateTime? TimeStamp { get; set; }
    public DateTime? Date { get; set; }

    /// <summary>
    /// Type of event - Issue, Claim, Dispute, Cancel, MarkAsPaid, PaymentApproved, Closed
    /// </summary>
    /// <remarks>Gmc.Cloud.EagleArchive.Invoices.InvoiceEventType</remarks>
    public int? Type { get; set; }

    public MoneyInfo? Amount { get; set; }

    /// <summary>
    /// Payable amount invoice attribute in time of issue event
    /// </summary>
    public MoneyInfo? PayableAmount { get; set; }

    public string? Text { get; set; }
    public int? PaymentMeans { get; set; }
    public string? UserFullName { get; set; }

    /// <summary>
    /// Event invoker of type user. Mutually exclusive with <exception cref="CustomerClientId"></exception>
    /// </summary>
    public ulong? UserId { get; set; }

    /// <summary>
    /// Event invoker of type client. Mutually exclusive with <exception cref="UserId"></exception>
    /// </summary>
    public string? CustomerClientId { get; set; }

    public TransactionInfo? TransactionInfo { get; set; }

    /// <summary>
    /// Data relevant only for event type CommunicationSubmission
    /// </summary>
    public CommunicationSubmissionEventData? CommunicationSubmissionEventData { get; set; }

    /// <summary>
    /// Data relevant only for event type StateChanged
    /// </summary>
    public StateChangeEventData? StateChangeEventData { get; set; }

    /// <summary>
    /// Name of the blob containing metadata for state when event was created
    /// </summary>
    public string? QiifBlobName { get; set; }
}
