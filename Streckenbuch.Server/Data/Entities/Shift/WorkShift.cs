using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Shift;

public class WorkShift : IEntity
{
    public Guid Id { get; set; }
 
    public Guid WorkDriverId { get; set; }
    
    public WorkDriver WorkDriver { get; set; }
    
    public DateOnly Datum { get; set; }

    public List<WorkTrip> WorkTrips { get; set; } = new();
}