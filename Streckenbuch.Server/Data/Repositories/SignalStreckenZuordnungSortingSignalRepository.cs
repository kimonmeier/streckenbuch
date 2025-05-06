using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class SignalStreckenZuordnungSortingSignalRepository : GenericRepository<SignalStreckenZuordnungSortingSignal>
{
    public SignalStreckenZuordnungSortingSignalRepository(DbContext dbContext) : base(dbContext)
    {
    }
}