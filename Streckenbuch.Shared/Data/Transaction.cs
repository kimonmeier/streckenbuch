using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Streckenbuch.Shared.Data;

public class Transaction : IDisposable
{
    private DbContext _context;
    private IDbContextTransaction? _transaction;
    private ILogger<Transaction> _logger;

    public Transaction(ILogger<Transaction> logger, DbContext context)
    {
        _logger = logger;
        _context = context;

        _transaction = _context.Database.BeginTransaction();
        _logger.LogDebug("Transaction {0} started", _transaction.TransactionId);
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Something went wrong, the underlying DatabaseTransaction is already closed!");
        }
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Saved changes to transaction {0}", _transaction.TransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "While saving the changes to the database, an exception occured!");
            throw;
        }
        try
        {
            await _transaction.CommitAsync(cancellationToken);
            _logger.LogDebug("Commited Transaction {0}", _transaction.TransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to commit transaction {0}", _transaction.TransactionId);
            throw;
        }

        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_transaction == null)
        {
            return;
        }

        _logger.LogWarning("Transaction not commited and it's gonna get reverted");
        _transaction.Rollback();
        _transaction.Dispose();

        _logger.LogInformation("Database Entries are reloaded to wipe the changes");
        _context.ChangeTracker.DetectChanges();
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.Reload();
        }
    }
}
