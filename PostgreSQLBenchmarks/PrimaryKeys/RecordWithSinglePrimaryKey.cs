using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PostgreSQLBenchmarks.PrimaryKeys;

[Index(nameof(TenantId))]
public class RecordWithSinglePrimaryKey
{
    public int TenantId { get; set; }
    
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
    public int RecordWithSinglePrimaryKeyId { get; set; }
    
    public required string Name { get; set; }

    public virtual Tenant Tenant { get; set; } = default!;
}
