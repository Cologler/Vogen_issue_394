using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Vogen;

namespace Vogen_issue_394;

internal class SomeDbContext : DbContext
{
    public DbSet<SomeEntity> SomeEntities { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("SomeDB");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SomeId>()
            .HaveConversion<SomeId.EfCoreValueConverter>();
    }
}

[ValueObject(conversions: Conversions.EfCoreValueConverter)]
[Instance("Unset", 0)]
public partial struct SomeId { }

public class SomeEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public SomeId SomeId { get; set; }
}
