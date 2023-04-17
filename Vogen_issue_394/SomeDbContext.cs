using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Vogen;

namespace Vogen_issue_394;

internal class SomeDbContext : DbContext
{
    public DbSet<SomeEntity> SomeEntities { get; set; } = default!;

    // you can use this method explicitly when creating your entities, or use SomeIdValueGenerator as shown below
    // public int GetNextMyEntityId()
    // {
    //     var maxLocalId = SomeEntities.Local.Any() ? SomeEntities.Local.Max(e => e.Id.Value) : 0;
    //     var maxSavedId = SomeEntities.Any() ? SomeEntities.Max(e => e.Id.Value) : 0;
    //     return Math.Max(maxLocalId, maxSavedId) + 1;
    // }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SomeEntity>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(e => e.Id).HasValueGenerator<SomeIdValueGenerator>();
                b.Property(e => e.Id).HasConversion(
                    v => v.Value,
                    v => SomeId.From(v));
            });
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("SomeDB");
    }
}

internal class SomeIdValueGenerator : ValueGenerator<SomeId>
{
    public override SomeId Next(EntityEntry entry)
    {
        var entities = ((SomeDbContext)entry.Context).SomeEntities;
        
        var next = Math.Max(maxFrom(entities.Local), maxFrom(entities)) + 1;
        
        return SomeId.From(next);

        static int maxFrom(IEnumerable<SomeEntity> es)
        {
            return es.Any() ? es.Max(e => e.Id.Value) : 0;
        }
    }

    public override bool GeneratesTemporaryValues => false;
}

[ValueObject]
//[ValueObject(conversions: Conversions.EfCoreValueConverter)]
[Instance("Unset", -1)]
public partial class SomeId
{
}


public class SomeEntity
{
    public SomeId Id { get; set; } = null!; // must be null in order for EF core to generate a value
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
