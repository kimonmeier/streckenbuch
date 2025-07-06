using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Shift;

namespace Streckenbuch.Server.Data.Configuration;

public class WorkDriverConfiguration : IEntityTypeConfiguration<WorkDriver>
{
    public void Configure(EntityTypeBuilder<WorkDriver> builder)
    {
        builder
            .ToTable(nameof(WorkDriver));

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.ApplicationUser)
            .WithOne()
            .HasForeignKey<WorkDriver>(x => x.ApplicationUserId);
        
        builder
            .HasIndex(x => x.TrainDriverNumber)
            .IsUnique();
    }
}