using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Data.Configuration;

public class SignalStreckenZuordnungSortingStreckeConfiguration : IEntityTypeConfiguration<SignalStreckenZuordnungSortingStrecke>
{
    public void Configure(EntityTypeBuilder<SignalStreckenZuordnungSortingStrecke> builder)
    {
        builder
            .ToTable(nameof(SignalStreckenZuordnungSortingStrecke));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(nameof(SignalStreckenZuordnungSortingStrecke.GueltigVon), nameof(SignalStreckenZuordnungSortingStrecke.GueltigBis));
    }
}