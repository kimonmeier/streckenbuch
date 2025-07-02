using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class WorkTripRepository : GenericRepository<WorkTrip>
{
    public WorkTripRepository(DbContext dbContext) : base(dbContext)
    {
    }
}