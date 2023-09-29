using System;
using System.Collections.Generic;
using BTDB.ODBLayer;

#nullable enable

namespace BtdbBenchmarks.ManyRecords;

public class CommunicationPieceMetadata
{
    [PrimaryKey(1)]
    public ulong CompanyId { get; set; }
        
    [PrimaryKey(2)]
    [SecondaryKey ("CommunicationPieceId")]
    public ulong CommunicationPieceId { get; set; }
        
    [SecondaryKey ("ArchiveId")]
    public string ArchiveId { get; set; }
        
    public IList<MetadataFile> Files { get; set; }
        
    public DateTime CreatedDate { get; set; }
        
    public CommunicationPieceMetadata(ulong companyId, ulong communicationPieceId, string archiveId, DateTime createdDate)
    {
        CompanyId = companyId;
        CommunicationPieceId = communicationPieceId;
        ArchiveId = archiveId;
        Files = new List<MetadataFile>();
        CreatedDate = createdDate;
    }
        
    [Obsolete]
    public CommunicationPieceMetadata() {
        ArchiveId = null!;
        Files = new List<MetadataFile>();
    }
}
    
public class MetadataFile
{
    public string FileNameWithPath { get; set; }

    public string Location { get; set; }

    MetadataFile(string fileNameWithPath, string location)
    {
        FileNameWithPath = fileNameWithPath;
        Location = location;
    }

    [Obsolete("Only for infra")]
    public MetadataFile()
    {
        FileNameWithPath = null!;
        Location = null!;
    }

    public static MetadataFile ProcessedAckFile(string fileName, string location) => new ($"ProcessedAckFiles/{fileName}", location);
    public static MetadataFile FailedAckFile(string fileName, string location) => new ($"FailedAckFiles/{fileName}", location);
    public static MetadataFile PostponedAckFile(string fileName, string location) => new ($"PostponedAckFiles/{fileName}", location);
}
    
public interface ICommunicationPieceMetadataTable : IRelation<CommunicationPieceMetadata>
{
    CommunicationPieceMetadata? FindByCommunicationPieceIdOrDefault(ulong communicationPieceId);
    CommunicationPieceMetadata? FindByArchiveIdOrDefault(string archiveId);
    bool RemoveById(ulong companyId, ulong communicationPieceId);
}