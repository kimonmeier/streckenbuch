using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data;

namespace Streckenbuch.Server.Background;

public class ShutdownService : IHostedService
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ApplicationDbContext _dbContext;

    public ShutdownService(IHostApplicationLifetime appLifetime, ApplicationDbContext dbContext)
    {
        _appLifetime = appLifetime;
        _dbContext = dbContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStopping.Register(OnShutdown);

        return Task.CompletedTask;
    }

    private void OnShutdown()
    {
        _dbContext.Database.CloseConnection();
        _dbContext.Dispose();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}