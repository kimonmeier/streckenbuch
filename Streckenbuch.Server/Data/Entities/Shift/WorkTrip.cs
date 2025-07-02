using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Shift;

public class WorkTrip : IEntity
{
    public Guid Id { get; set; }
    
    public Guid WorkShiftId { get; set; }
    
    public WorkShift WorkShift { get; set; }
    
    public int TripNumber { get; set; }
}