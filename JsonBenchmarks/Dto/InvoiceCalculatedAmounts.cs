namespace JsonBenchmarks.Dto;

public record InvoiceCalculatedAmounts
{
    public MoneyInfo? MarkedAsPaidAmount { get; set; }
    public MoneyInfo? ConfirmedAsPaidAmount { get; set; }
    public MoneyInfo? RemainingAmount { get; set; }

    /// <summary>
    /// Int of <see cref="InvoiceRemainingAmountStatus"/>
    /// </summary>
    public int? RemainingAmountStatus { get; set; }

    public MoneyInfo? PendingPaymentAmount { get; set; }
}
