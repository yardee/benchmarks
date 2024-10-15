namespace JsonBenchmarks.Dto;

public record CommunicationAttributeInfo
{
    public ulong? AttributeId { get; set; }
    public string? AttributeIdentifier { get; set; }
    public string? AttributeName { get; set; }

    /// <summary>
    /// Attribute value serialized into json
    /// </summary>
    public string? AttributeMetadata { get; set; }
}
