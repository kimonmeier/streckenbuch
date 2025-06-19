using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Linien;

namespace Streckenbuch.Server.Data.Configuration;

public class LinieTrainConfiguration : IEntityTypeConfiguration<LinieTrain>
{
    public void Configure(EntityTypeBuilder<LinieTrain> builder)
    {
        builder
            .ToTable(nameof(LinieTrain));

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Linie)
            .WithOne()
            .HasForeignKey<LinieTrain>(x => x.LinieId);

        builder
            .HasIndex(x => x.TrainNumber);
    }
}