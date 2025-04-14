using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class LinienKonfigurationRepository : GenericRepository<LinienStreckenKonfigurationen>
{
    public LinienKonfigurationRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<LinienStreckenKonfigurationen>> ListByLinie(Guid linieId)
    {
        return Entities.Where(x => x.LinieId.Equals(linieId)).OrderBy(x => x.Order).ToListAsync();
    }
}
