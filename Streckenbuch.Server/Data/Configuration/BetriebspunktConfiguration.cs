using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;

namespace Streckenbuch.Server.Data.Configuration;

public class BetriebspunktConfiguration : IEntityTypeConfiguration<Betriebspunkt>
{
    public void Configure(EntityTypeBuilder<Betriebspunkt> builder)
    {
        builder
            .ToTable(nameof(Betriebspunkt));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .OwnsOne(
            x => x.Location,
                x =>
                {
                    x.Ignore(x => x.CoordinateValue);
                    x.Ignore(x => x.Z);
                    x.Ignore(x => x.M);
                    x.Ignore(x => x.IsValid);
                }
       );

        builder
            .Property(x => x.Typ)
            .HasConversion<short>();

        builder
            .Property(x => x.Kommentar)
            .HasMaxLength(1024);

        builder
            .Property(x => x.Name)
            .HasMaxLength(64);
    }
}
