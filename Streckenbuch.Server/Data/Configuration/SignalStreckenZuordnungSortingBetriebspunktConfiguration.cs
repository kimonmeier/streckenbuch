using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Data.Configuration;

public class SignalStreckenZuordnungSortingBetriebspunktConfiguration : IEntityTypeConfiguration<SignalStreckenZuordnungSortingBetriebspunkt>
{
    public void Configure(EntityTypeBuilder<SignalStreckenZuordnungSortingBetriebspunkt> builder)
    {
        builder
            .ToTable(nameof(SignalStreckenZuordnungSortingBetriebspunkt));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .HasOne(x => x.BetriebspunktStreckenZuordnung)
            .WithMany()
            .HasForeignKey(x => x.BetriebspunktStreckenZuordnungId);

        builder
            .HasOne(x => x.SignalStreckenZuordnungSortingStrecke)
            .WithMany(x => x.Betriebspunkte)
            .HasForeignKey(x => x.SignalStreckenZuordnungSortingStreckeId);
    }
}