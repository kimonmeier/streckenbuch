using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Data.Configuration;

public class SignalConfiguration : IEntityTypeConfiguration<Signal>
{
    public void Configure(EntityTypeBuilder<Signal> builder)
    {
        builder
            .ToTable(nameof(Signal));

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
            .Property(x => x.Seite)
            .HasConversion<int>();

        builder
            .Property(x => x.Typ)
            .HasConversion<int>();

        builder
            .HasOne(x => x.Betriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BetriebspunktId);
    }
}
