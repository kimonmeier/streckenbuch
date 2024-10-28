using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Data.Configuration;

public class SignalStreckenZuordnungConfiguration : IEntityTypeConfiguration<SignalStreckenZuordnung>
{
    public void Configure(EntityTypeBuilder<SignalStreckenZuordnung> builder)
    {
        builder
            .ToTable(nameof(SignalStreckenZuordnung));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Strecke)
            .WithMany()
            .HasForeignKey(x => x.StreckeId);

        builder
            .HasOne(x => x.Signal)
            .WithMany()
            .HasForeignKey(x => x.SignalId);

        builder
            .HasIndex(nameof(SignalStreckenZuordnung.StreckeId), nameof(SignalStreckenZuordnung.SignalId));
    }
}
