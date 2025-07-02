using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streckenbuch.Server.Data.Entities.Shift;

namespace Streckenbuch.Server.Data.Configuration;

public class TripRecordingConfiguration : IEntityTypeConfiguration<TripRecording>
{
    public void Configure(EntityTypeBuilder<TripRecording> builder)
    {
        builder
            .ToTable(nameof(TripRecording));
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .HasOne(x => x.WorkTrip)
            .WithMany()
            .HasForeignKey(x => x.WorkTripId);
        
        builder
            .OwnsOne(
                x => x.Location,
                x =>
                {
                    x.Ignore(x => x.CoordinateValue);
                    x.Ignore(x => x.Z);
                    x.Ignore(x => x.M);
                    x.Ignore(x => x.IsValid);
                }
            );
    }
}