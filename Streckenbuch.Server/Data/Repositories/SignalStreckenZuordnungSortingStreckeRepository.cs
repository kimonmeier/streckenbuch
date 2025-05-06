using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class SignalStreckenZuordnungSortingStreckeRepository : GenericRepository<SignalStreckenZuordnungSortingStrecke>
{
    public SignalStreckenZuordnungSortingStreckeRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
    public Task<SignalStreckenZuordnungSortingStrecke?> GetWithChild(Guid streckenZuordnungId)
    {
        return Entities
            .Include(x => x.Betriebspunkte)
            .ThenInclude(x => x.Signale)
            .SingleOrDefaultAsync(x => x.Id == streckenZuordnungId);
    }
    
    public Task<List<SignalStreckenZuordnungSortingStrecke>> GetByStreckeKonfigurationIdWithChild(Guid streckeKonfigurationId)
    {
        return Entities
            .Include(x => x.Betriebspunkte)
            .ThenInclude(x => x.Signale)
            .Where(x => x.StreckenKonfigurationId == streckeKonfigurationId)
            .ToListAsync();
    }

    public Task<List<SignalStreckenZuordnungSortingStrecke>> ListByStreckeKonfigurationId(Guid streckeKonfigurationId)
    {
        return Entities
            .Where(x => x.StreckenKonfigurationId == streckeKonfigurationId)
            .ToListAsync();
    }
}