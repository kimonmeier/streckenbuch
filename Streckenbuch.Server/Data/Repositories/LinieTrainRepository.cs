using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class LinieTrainRepository : GenericRepository<LinieTrain>
{
    public LinieTrainRepository(DbContext dbContext) : base(dbContext)
    {
    }
}