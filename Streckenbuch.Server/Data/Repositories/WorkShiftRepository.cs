using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class WorkShiftRepository : GenericRepository<WorkShift>
{
    public WorkShiftRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
    public Task<WorkShift?> FindByDateAsync(DateOnly date)
    {
        return Entities.SingleOrDefaultAsync(x => x.Datum == date);
    }

    public Task<List<WorkShift>> FindByDriverWithTrips(Guid driverId)
    {
        return Entities.Include(x => x.WorkTrips).Where(x => x.WorkDriverId.Equals(driverId)).OrderBy(x => x.Datum).ToListAsync();
    }
}