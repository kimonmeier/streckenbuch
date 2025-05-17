using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Halteorte;

namespace Streckenbuch.Server.Data.Configuration;

public class HalteortPositionConfiguration : IEntityTypeConfiguration<HalteortPositions>
{
    public void Configure(EntityTypeBuilder<HalteortPositions> builder)
    {
        builder
            .ToTable(nameof(HalteortPositions));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.HalteortHead)
            .WithMany()
            .HasForeignKey(x => x.HalteortHeadId);

        builder
            .Property(x => x.Typ)
            .HasConversion<int>();

        builder
            .Property(x => x.Description)
            .HasMaxLength(255);
    }
}