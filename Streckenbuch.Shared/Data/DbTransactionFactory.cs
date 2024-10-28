using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Streckenbuch.Shared.Data;

public class DbTransactionFactory
{
    private readonly DbContext _context;
    private readonly ILogger<Transaction> _logger;

    public DbTransactionFactory(ILogger<Transaction> logger, DbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Transaction CreateTransaction()
    {
        return new Transaction(_logger, _context);
    }
}
