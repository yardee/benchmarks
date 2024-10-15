namespace JsonBenchmarks.Dto;

public record CommunicationPreparationChannelData
{
    public string? OutputChannel { get; set; }
    public ulong? OutputUserId { get; set; }
}
