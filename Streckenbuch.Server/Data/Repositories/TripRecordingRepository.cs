using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class TripRecordingRepository : GenericRepository<TripRecording>
{
    public TripRecordingRepository(DbContext dbContext) : base(dbContext)
    {
    }
}