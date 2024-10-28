using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Strecken;

namespace Streckenbuch.Server.Data.Configuration;

public class StreckenKonfigurationConfiguration : IEntityTypeConfiguration<StreckenKonfiguration>
{
    public void Configure(EntityTypeBuilder<StreckenKonfiguration> builder)
    {
        builder
            .ToTable(nameof(StreckenKonfiguration));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.VonBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.VonBetriebspunktId);

        builder
            .HasOne(x => x.BisBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BisBetriebspunktId);

        builder
            .HasOne(x => x.Strecke)
            .WithMany()
            .HasForeignKey(x => x.StreckeId);

        builder
            .HasIndex(nameof(StreckenKonfiguration.StreckeId), nameof(StreckenKonfiguration.VonBetriebspunktId), nameof(StreckenKonfiguration.BisBetriebspunktId))
            .IsUnique();
    }
}
