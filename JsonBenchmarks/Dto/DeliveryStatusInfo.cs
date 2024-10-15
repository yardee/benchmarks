namespace JsonBenchmarks.Dto;

public record DeliveryStatusInfo
{
    /// <summary>
    /// Type of status - EnumValue, SubServiceIdentifier, IncidentSeverity
    /// </summary>
    public int Type { get; set; }

    public required string DeliveryStatus { get; set; }
}
