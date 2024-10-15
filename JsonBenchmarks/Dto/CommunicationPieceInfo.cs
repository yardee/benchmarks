#nullable enable

using Newtonsoft.Json;

namespace JsonBenchmarks.Dto;

public interface ICommunicationPieceInfo
{
    ulong CompanyId { get; }
    ulong CreatedByUserId { get; }
    ulong CommunicationPieceId { get; }
    DateTime CreationDate { get; }
    string? CustomerClientId { get; }
}

public class CommunicationPieceInfo 
{
    [JsonIgnore]
    public ObjectType ObjectType => ObjectType.CommunicationPiece;

    [JsonIgnore]
    public ulong ObjectId => CommunicationPieceId;

    public required ulong CompanyId { get; init; }
    public required ulong CreatedByUserId { get; init; }
    public required ulong CommunicationPieceId { get; init; }
    public required DateTime CreationDate { get; init; }
    public required uint ArchiveDurationInMonths { get; init; }
    public required DateTime ArchiveExpirationDate { get; set; }
    public required string ArchiveId { get; init; }
    public required string? CustomerClientId { get; init; }
    public required string? Channel { get; init; }
    public required ulong? TeamId { get; init; }
}
