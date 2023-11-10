using Microsoft.EntityFrameworkCore;

namespace PostgreSQLBenchmarks.PrimaryKeys;

class KeysDbContext: DbContext
{
    public DbSet<Tenant> Tenants { get; set; } = default!;
    public DbSet<RecordWithForeignKeyInPrimaryKey> RecordWithForeignKeyInPrimaryKeys { get; set; } = default!;
    public DbSet<RecordWithSinglePrimaryKey> RecordWithSinglePrimaryKeys { get; set; } = default!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Database=primarykeysbenchmark;Username=pgadmin;Password=admin;Port=5432");
}
