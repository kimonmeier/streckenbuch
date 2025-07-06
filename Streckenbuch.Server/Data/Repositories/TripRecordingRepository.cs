using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class TripRecordingRepository : GenericRepository<TripRecording>
{
    public TripRecordingRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<TripRecording>> FindByTrip(Guid tripId)
    {
        return Entities.Where(x => x.WorkTripId.Equals(tripId)).OrderBy(x => x.Time).ToListAsync();
    } 
}