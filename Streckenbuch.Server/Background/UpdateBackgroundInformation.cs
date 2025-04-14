﻿
using Grpc.Core;
using Streckenbuch.Shared.Services;
using System.Collections.Concurrent;

namespace Streckenbuch.Server.Background;

public class UpdateBackgroundInformation : BackgroundService
{
    private ConcurrentDictionary<int, List<IServerStreamWriter<StartStreamResponse>>> _connections;

    public UpdateBackgroundInformation()
    {
        _connections = new ConcurrentDictionary<int, List<IServerStreamWriter<StartStreamResponse>>>();
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

    public void RegisterClient(int trainNumber, IServerStreamWriter<StartStreamResponse> client)
    {
        if (!_connections.ContainsKey(trainNumber))
        {
            _connections[trainNumber] = new List<IServerStreamWriter<StartStreamResponse>>();
        }

        _connections[trainNumber].Add(client);
    }

    public void UnregisterClient(int trainNumber, IServerStreamWriter<StartStreamResponse> client)
    {
        _connections[trainNumber].Remove(client);
    }
}
