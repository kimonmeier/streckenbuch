
using Grpc.Core;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Background;

public class UpdateBackgroundInformation : BackgroundService
{
    private Dictionary<int, List<IServerStreamWriter<StartStreamRepsonse>>> _connections;

    public UpdateBackgroundInformation()
    {
        _connections = new Dictionary<int, List<IServerStreamWriter<StartStreamRepsonse>>>();
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

        while(!stoppingToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync();

            //TODO: Add Logic
        }
    }

    public void RegisterClient(int trainNumber, IServerStreamWriter<StartStreamRepsonse> client)
    {
        if (!_connections.ContainsKey(trainNumber))
        {
            _connections[trainNumber] = new List<IServerStreamWriter<StartStreamRepsonse>>();
        }

        _connections[trainNumber].Add(client);
    }

    public void UnregisterClient(int trainNumber, IServerStreamWriter<StartStreamRepsonse> client)
    {
        _connections[trainNumber].Remove(client);
    }
}
