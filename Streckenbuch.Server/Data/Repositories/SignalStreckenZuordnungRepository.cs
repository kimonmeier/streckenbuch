using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class SignalStreckenZuordnungRepository : GenericRepository<SignalStreckenZuordnung>
{
    public SignalStreckenZuordnungRepository(DbContext dbContext) : base(dbContext)
    {
    }


    public async Task<Guid> FindZuordnungBySignalAndStrecke(Guid signalId, Guid streckeId)
    {
        return (await Entities.SingleAsync(x => x.SignalId == signalId && x.StreckeId == streckeId)).Id;
    }

    public Task<List<SignalStreckenZuordnung>> ListBySignalId(Guid signalId)
    {
        return Entities.Where(x => x.SignalId == signalId).ToListAsync();
    }

    public Task<List<SignalStreckenZuordnung>> ListByBetriebspunktAndStrecke(Guid betriebspunktId, Guid streckenkonfigurationId)
    {
        return Entities.Include(x => x.Signal).Where(x => x.Strecke.BetriebspunktId == betriebspunktId && x.Strecke.StreckenKonfigurationId == streckenkonfigurationId).ToListAsync();
    }
}
