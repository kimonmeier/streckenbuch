using Streckenbuch.Miku;
using Streckenbuch.Miku.Models.Fahrten;
using Streckenbuch.Server.States;

namespace Streckenbuch.Server.Background;

public class UpdateBackgroundInformation : BackgroundService
{
    private ILogger<UpdateBackgroundInformation> _logger;
    private ContinuousConnectionState _continuousConnectionState;
    private readonly MikuApi _mikuApi;

    public UpdateBackgroundInformation(ILogger<UpdateBackgroundInformation> logger, ContinuousConnectionState continuousConnectionState, MikuApi mikuApi)
    {
        _logger = logger;
        _continuousConnectionState = continuousConnectionState;
        _mikuApi = mikuApi;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (!stoppingToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync(stoppingToken);

            var clients = _continuousConnectionState.GetRegisteredClients();
            var registeredTrainOperators = _continuousConnectionState.GetRegisteredTrainOperator();
            _logger.LogInformation("Update background information with {0} Clients on {1} trains connected", clients.Count, registeredTrainOperators.Count);

            List<Task> tasksExecuting = new List<Task>();
            foreach (KeyValuePair<Guid, int> trainOperator in registeredTrainOperators)
            {
                tasksExecuting.Add(UpdateInformationForTrain(trainOperator.Key, trainOperator.Value));
            }

            await Task.WhenAll(tasksExecuting);            
            _logger.LogInformation("Sucessfully updated background for {0} trains", registeredTrainOperators.Count);
        }
    }
    
    private async Task UpdateInformationForTrain(Guid clientId, int trainNumber)
    {
        try
        {
            var fahrtInformation = await _mikuApi.Fahrt.ListByTrainNumber(trainNumber);

            await _continuousConnectionState.ProcessMikuInformation(clientId, fahrtInformation.Haltestellen);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while updating background information for train {0}", trainNumber);
        }
    }

}