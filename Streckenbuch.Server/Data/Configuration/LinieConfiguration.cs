using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Linien;

namespace Streckenbuch.Server.Data.Configuration;

public class LinieConfiguration : IEntityTypeConfiguration<Linie>
{
    public void Configure(EntityTypeBuilder<Linie> builder)
    {
        builder
            .ToTable(nameof(Linie));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Typ)
            .HasConversion<short>();

        builder
            .HasOne(x => x.VonBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.VonBetriebspunktId);

        builder
            .HasOne(x => x.BisBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BisBetriebspunktId);

        builder
            .HasIndex([nameof(Linie.Typ), nameof(Linie.Nummer), nameof(Linie.VonBetriebspunktId), nameof(Linie.BisBetriebspunktId)])
            .IsUnique();
    }
}
