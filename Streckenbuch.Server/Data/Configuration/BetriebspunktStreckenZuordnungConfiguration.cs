using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;

namespace Streckenbuch.Server.Data.Configuration;

public class BetriebspunktStreckenZuordnungConfiguration : IEntityTypeConfiguration<BetriebspunktStreckenZuordnung>
{
    public void Configure(EntityTypeBuilder<BetriebspunktStreckenZuordnung> builder)
    {
        builder
            .ToTable(nameof(BetriebspunktStreckenZuordnung));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Betriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BetriebspunktId);

        builder
            .HasOne(x => x.StreckenKonfiguration)
            .WithMany()
            .HasForeignKey(x => x.StreckenKonfigurationId);
        /*
        builder
            .HasIndex(nameof(BetriebspunktStreckenZuordnung.StreckenKonfigurationId), nameof(BetriebspunktStreckenZuordnung.BetriebspunktId))
            .IsUnique();

        builder
            .HasIndex(nameof(BetriebspunktStreckenZuordnung.StreckenKonfigurationId), nameof(BetriebspunktStreckenZuordnung.SortNummer))
            .IsUnique();*/
    }
}
