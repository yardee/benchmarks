namespace JsonBenchmarks.Dto;

public record InvoiceInfo
{
    /// <summary>
    /// Invoice, Credit note
    /// </summary>
    public int? InvoiceType { get; set; }

    /// <summary>
    /// Invoice current state - Issued, Claimed, Disputed, Cancelled, MarkedAsPartiallyPaid, MarkedAsPaid, PartiallyPaid, Paid, Closed
    /// </summary>
    /// <remarks>Gmc.Cloud.Eagle.Common.Db.InvoiceState</remarks>
    public int? Status { get; set; }

    public MoneyInfo? Amount { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? InvoiceDueDate { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceFirstPaymentDate { get; set; }
    public DateTime? InvoiceFirstMarkedAsPaidDate { get; set; }
    public DateTime? InvoiceLastPaymentDate { get; set; }
    public DateTime? InvoiceLastMarkedAsPaidDate { get; set; }
    public DateTime? InvoiceClosedDate { get; set; }
    public DateTime? InvoicePaidDate { get; set; }

    public string? InvoiceCurrency { get; set; }
    public string? InvoiceTaxCurrency { get; set; }
    public string? InvoiceSellerAddressCountry { get; set; }
    public string? InvoiceBuyerAddressCountry { get; set; }
    public string? InvoicePurchaseOrder { get; set; }
    public string? InvoiceSellerId { get; set; }
    public string? InvoiceBuyerId { get; set; }
    public MoneyInfo? InvoiceTotalAmountWithoutTax { get; set; }
    public MoneyInfo? InvoicePayableAmount { get; set; }
    public InvoiceCalculatedAmounts? InvoiceCalculatedAmounts { get; set; }
    public bool? InvoiceIsLastCommunicationPerDeliveryService { get; set; }

    /// <summary>
    /// All invoice events to view history.
    /// </summary>
    public IList<InvoiceEvent>? Events { get; set; }

    /// <summary>
    /// List of attributes
    /// </summary>
    public IList<CommunicationAttributeInfo>? Attributes { get; set; }
}
