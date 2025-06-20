using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class BetriebspunkteRepository : GenericRepository<Betriebspunkt>
{
    public BetriebspunkteRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<Guid>> ListIdsByMikuIds(List<int> mikuIds)
    {
        return Entities.Join(mikuIds, x => x.MikuId, y => y, (x, y) => x.Id).ToListAsync();
    }

    public async Task<Guid> GetIdByMikuId(int mikuId)
    {
        return (await Entities.FirstOrDefaultAsync(x => x.MikuId == mikuId))?.Id ?? Guid.Empty;
    }
}
