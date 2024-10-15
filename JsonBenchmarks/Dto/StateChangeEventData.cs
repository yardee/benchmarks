namespace JsonBenchmarks.Dto;

public record StateChangeEventData
{
    /// <summary>
    /// New state of invoice - value is of type <see cref="Gmc.Cloud.Eagle.Common.Db.InvoiceState"/>
    /// </summary>
    public int? InvoiceState { get; set; }
}
