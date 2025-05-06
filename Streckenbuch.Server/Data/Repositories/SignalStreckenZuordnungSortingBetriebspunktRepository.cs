using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class SignalStreckenZuordnungSortingBetriebspunktRepository : GenericRepository<SignalStreckenZuordnungSortingBetriebspunkt>
{
    public SignalStreckenZuordnungSortingBetriebspunktRepository(DbContext dbContext) : base(dbContext)
    {
    }
}