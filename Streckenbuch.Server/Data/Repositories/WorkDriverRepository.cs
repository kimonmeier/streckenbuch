using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class WorkDriverRepository : GenericRepository<WorkDriver>
{
    public WorkDriverRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<WorkDriver?> FindByDriverNumberAsync(int trainDriverNumber)
    {
        return Entities.SingleOrDefaultAsync(x => x.TrainDriverNumber == trainDriverNumber);
    }
    
    public Task<WorkDriver?> FindByApplicationUserAsync(string applicationUserId)
    {
        return Entities.SingleOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);
    }
}