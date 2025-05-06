using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Data.Configuration;

public class SignalStreckenZuordnungSortingSignalConfiguration : IEntityTypeConfiguration<SignalStreckenZuordnungSortingSignal>
{
    public void Configure(EntityTypeBuilder<SignalStreckenZuordnungSortingSignal> builder)
    {
        builder
            .ToTable(nameof(SignalStreckenZuordnungSortingSignal));
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Signal)
            .WithMany()
            .HasForeignKey(x => x.SignalId);
        
        builder
            .HasOne(x => x.SignalStreckenZuordnungSortingBetriebspunkt)
            .WithMany(x => x.Signale)
            .HasForeignKey(x => x.SignalStreckenZuordnungSortingBetriebspunktId);
    }
}