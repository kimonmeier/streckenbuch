using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Halteorte;

namespace Streckenbuch.Server.Data.Configuration;

public class HalteortHeadConfiguration : IEntityTypeConfiguration<HalteortHead>
{
    public void Configure(EntityTypeBuilder<HalteortHead> builder)
    {
        builder
            .ToTable(nameof(HalteortHead));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .HasOne(x => x.Betriebspunkt)
            .WithMany()
            .HasForeignKey(x => x.BetriebspunktId);
        
        
    }
}