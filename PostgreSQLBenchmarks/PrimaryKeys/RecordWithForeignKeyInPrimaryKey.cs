using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PostgreSQLBenchmarks.PrimaryKeys;

[PrimaryKey(nameof(TenantId), nameof(RecordWithForeignKeyInPrimaryKeyId))]
public class RecordWithForeignKeyInPrimaryKey
{
    public int TenantId { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
    public int RecordWithForeignKeyInPrimaryKeyId { get; set; }
    
    public required string Name { get; set; }

    public virtual Tenant Tenant { get; set; } = default!;
}
