namespace PostgreSQLBenchmarks.PrimaryKeys;

public class Tenant
{
    public int TenantId { get; set; }
    public required string Name { get; set; }
    
    public List<RecordWithForeignKeyInPrimaryKey> RecordWithForeignKeyInPrimaryKeys { get; } = new();
    public List<RecordWithSinglePrimaryKey> RecordWithSinglePrimaryKeys { get; } = new();
}
