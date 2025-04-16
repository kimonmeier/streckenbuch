using Streckenbuch.Server.States;

namespace Streckenbuch.Server.Background;

public class UpdateBackgroundInformation : BackgroundService
{
    private ILogger<UpdateBackgroundInformation> _logger;
    private ContinuousConnectionState _continuousConnectionState;

    public UpdateBackgroundInformation(ILogger<UpdateBackgroundInformation> logger, ContinuousConnectionState continuousConnectionState)
    {
        _logger = logger;
        _continuousConnectionState = continuousConnectionState;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (!stoppingToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync();

            var clients = _continuousConnectionState.GetRegisteredClients();
            var registeredTrainOperators = _continuousConnectionState.GetRegisteredTrainOperator();
            _logger.LogInformation("Update background information with {0} Clients on {1} trains connected", clients.Count, registeredTrainOperators.Count);
            
            //TODO: Add Logic
        }
    }

}