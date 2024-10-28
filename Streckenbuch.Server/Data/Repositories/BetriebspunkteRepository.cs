using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class BetriebspunkteRepository : GenericRepository<Betriebspunkt>
{
    public BetriebspunkteRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
