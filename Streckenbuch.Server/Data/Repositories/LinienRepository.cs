using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class LinienRepository : GenericRepository<Linie>
{
    public LinienRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
