using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Shift;

public class TripRecording : IEntity
{
    public Guid Id { get; set; }
    
    public Guid WorkTripId { get; set; }
    
    public WorkTrip WorkTrip { get; set; }
    
    public Coordinate Location { get; set; }
    
    public TimeOnly Time { get; set; }
}