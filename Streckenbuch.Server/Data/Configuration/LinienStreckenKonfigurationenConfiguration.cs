using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Linien;

namespace Streckenbuch.Server.Data.Configuration;

public class LinienStreckenKonfigurationenConfiguration : IEntityTypeConfiguration<LinienStreckenKonfigurationen>
{
    public void Configure(EntityTypeBuilder<LinienStreckenKonfigurationen> builder)
    {
        builder
            .ToTable(nameof(LinienStreckenKonfigurationen));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Linie)
            .WithMany()
            .HasForeignKey(x => x.LinieId);

        builder
            .HasOne(x => x.StreckenKonfiguration)
            .WithMany()
            .HasForeignKey(x => x.StreckenKonfigurationId);

        builder
            .HasOne(x => x.VonBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.VonBetriebspunktId);

        builder
            .HasOne(x => x.BisBetriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BisBetriebspunktId);

        builder
            .HasIndex([nameof(LinienStreckenKonfigurationen.LinieId), nameof(LinienStreckenKonfigurationen.StreckenKonfigurationId)])
            .IsUnique();
    }
}
