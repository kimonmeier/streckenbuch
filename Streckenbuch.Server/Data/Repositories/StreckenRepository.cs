using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class StreckenRepository : GenericRepository<Strecke>
{
    public StreckenRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
