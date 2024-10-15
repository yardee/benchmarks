namespace JsonBenchmarks.Dto;

public record MoneyInfo
{
    /// <summary>
    /// Minor value. Decimal converted to string. (Azure CosmosDb & search does not support decimals.
    /// </summary>
    public string? MinorValue { get; set; }

    /// <summary>
    /// Amount value. Decimal converted to string. (Azure CosmosDb & search does not support decimals.
    /// </summary>
    public string? AmountValue { get; set; }

    /// <summary>
    /// ISO 4217 currency code. 
    /// </summary>
    public string? CurrencyCode { get; set; }
}
