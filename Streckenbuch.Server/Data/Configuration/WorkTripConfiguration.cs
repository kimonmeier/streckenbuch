using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Shift;

namespace Streckenbuch.Server.Data.Configuration;

public class WorkTripConfiguration : IEntityTypeConfiguration<WorkTrip>
{
    public void Configure(EntityTypeBuilder<WorkTrip> builder)
    {
        builder
            .ToTable(nameof(WorkTrip));
        
        builder
            .HasKey(x => x.Id); 
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.WorkShift)
            .WithMany()
            .HasForeignKey(x => x.WorkShiftId);
    }
}