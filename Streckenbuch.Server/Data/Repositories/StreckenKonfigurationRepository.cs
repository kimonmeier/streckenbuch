using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class StreckenKonfigurationRepository : GenericRepository<StreckenKonfiguration>
{
    public StreckenKonfigurationRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<StreckenKonfiguration>> ListByStrecke(Guid streckeId)
    {
        return Entities
            .Where(x => x.StreckeId == streckeId)
            .Include(x => x.VonBetriebspunkt)
            .Include(x => x.BisBetriebspunkt)
            .ToListAsync();
    }

    public Task<StreckenKonfiguration> FindByIdIncludeBetriebspunkte(Guid id)
    {
        return Entities
            .Where(x => x.Id == id)
            .Include(x => x.VonBetriebspunkt)
            .Include(x => x.BisBetriebspunkt)
            .SingleAsync();
    }
}
