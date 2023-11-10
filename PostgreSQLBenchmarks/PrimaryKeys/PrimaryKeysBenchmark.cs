using BenchmarkDotNet.Attributes;

namespace PostgreSQLBenchmarks.PrimaryKeys;

[MemoryDiagnoser]
public class PrimaryKeysBenchmark
{
    private int _tenantId;
    private int _recordWithForeignKeyInPrimaryKeysId;
    private int _recordWithSinglePrimaryKeyId;

    [GlobalSetup]
    public void GlobalSetup()
    {
        using var context = new KeysDbContext();
        var tenant = new Tenant
        {
            Name = "Name"
        };
        context.Tenants.Add(tenant);

        foreach (var i in Enumerable.Range(0, 100))
        {
            context.Tenants.Add(new Tenant
            {
                Name = "Name"+i
            });
        }
        context.SaveChanges();
        _tenantId = tenant.TenantId;
        
        foreach (var i in Enumerable.Range(0, 100))
        {
            var anotherTenant = context.Tenants.Skip(1).First();
            
            context.RecordWithSinglePrimaryKeys.Add(new RecordWithSinglePrimaryKey
            {
                Tenant = tenant,
                Name = "Name"+i
            });
            
            context.RecordWithSinglePrimaryKeys.Add(new RecordWithSinglePrimaryKey
            {
                Tenant = anotherTenant,
                Name = "Name"+i
            });
            
            context.RecordWithForeignKeyInPrimaryKeys.Add(new RecordWithForeignKeyInPrimaryKey
            {
                Tenant = tenant,
                Name = "Name"+i
            });
            
            context.RecordWithForeignKeyInPrimaryKeys.Add(new RecordWithForeignKeyInPrimaryKey
            {
                Tenant = anotherTenant,
                Name = "Name"+i
            });
                        
            context.SaveChanges();
        }
        
        _recordWithForeignKeyInPrimaryKeysId = context.RecordWithForeignKeyInPrimaryKeys.Skip(50).First().RecordWithForeignKeyInPrimaryKeyId;
        _recordWithSinglePrimaryKeyId = context.RecordWithSinglePrimaryKeys.Skip(50).First().RecordWithSinglePrimaryKeyId;
    }
    
    [Benchmark]
    public List<RecordWithSinglePrimaryKey> List_RecordWithSinglePrimaryKeys()
    {
        using var context = new KeysDbContext();
        return context.RecordWithSinglePrimaryKeys
            .Where(x => x.TenantId == _tenantId)
            .ToList();
    }
    
    [Benchmark]
    public List<RecordWithForeignKeyInPrimaryKey> List_RecordWithForeignKeyInPrimaryKeys()
    {
        using var context = new KeysDbContext();
        return context.RecordWithForeignKeyInPrimaryKeys
            .Where(x => x.TenantId == _tenantId)
            .ToList();
    }
    
    [Benchmark]
    public RecordWithForeignKeyInPrimaryKey? Find_RecordWithForeignKeyInPrimaryKeys()
    {
        using var context = new KeysDbContext();
        return context.RecordWithForeignKeyInPrimaryKeys.Find(_tenantId, _recordWithForeignKeyInPrimaryKeysId);
    }
    
    [Benchmark]
    public RecordWithSinglePrimaryKey? Find_RecordWithSinglePrimaryKeys()
    {
        using var context = new KeysDbContext();
        return context.RecordWithSinglePrimaryKeys.Find(_recordWithSinglePrimaryKeyId);
    }
    
    [GlobalCleanup]
    public void Cleanup()
    {
        using var context = new KeysDbContext();
        foreach (var tenant in context.Tenants)
        {
            context.Tenants.Remove(tenant);
        }
        
        foreach (var recordWithSinglePrimaryKey in context.RecordWithSinglePrimaryKeys)
        {
            context.RecordWithSinglePrimaryKeys.Remove(recordWithSinglePrimaryKey);
        }
        
        foreach (var recordWithForeignKeyInPrimaryKey in context.RecordWithForeignKeyInPrimaryKeys)
        {
            context.RecordWithForeignKeyInPrimaryKeys.Remove(recordWithForeignKeyInPrimaryKey);
        }

        context.SaveChanges();
    }
}
