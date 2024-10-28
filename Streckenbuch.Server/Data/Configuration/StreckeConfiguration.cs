using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Strecken;

namespace Streckenbuch.Server.Data.Configuration;

public class StreckeConfiguration : IEntityTypeConfiguration<Strecke>
{
    public void Configure(EntityTypeBuilder<Strecke> builder)
    {
        builder
            .ToTable(nameof(Strecke));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(x => x.StreckenNummer)
            .IsUnique();
    }
}
