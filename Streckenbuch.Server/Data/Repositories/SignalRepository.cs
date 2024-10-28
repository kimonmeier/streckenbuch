using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class SignalRepository : GenericRepository<Signal>
{
    public SignalRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<Signal>> ListByBetriebspunktAsync(Guid betriebspunktId)
    {
        return Entities.Where(x => x.BetriebspunktId == betriebspunktId).ToListAsync();
    }
}
