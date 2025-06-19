using Grpc.Core;
using MediatR;
using Streckenbuch.Shared;
using Streckenbuch.Shared.Services;
using System.Text.Json;

namespace Streckenbuch.Client.States;

public class ContinuousConnectionState : IDisposable
{
    private readonly FahrenService.FahrenServiceClient _fahrenServiceClient;
    private readonly Guid _id;
    private readonly ISender _sender;
    private int? _registeredTrainNumber;

    public ContinuousConnectionState(FahrenService.FahrenServiceClient fahrenServiceClient, ISender sender)
    {
        _fahrenServiceClient = fahrenServiceClient;
        _sender = sender;
        _id = Guid.NewGuid();
    }

    public void StartBackgroundTask(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            while (!cancellationToken.IsCancellationRequested)
            {
                await periodicTimer.WaitForNextTickAsync(cancellationToken);

                var response = await _fahrenServiceClient.CaptureRealtimeMessagesAsync(new CaptureMessage()
                {
                    ClientId = _id,
                });
                
                _ = Task.Run(() =>
                {
                    ProcessMessages(response.Messages.ToList());
                }, cancellationToken).ConfigureAwait(false);
            }
        }, cancellationToken);
    }

    public async Task RegisterTrain(int trainNumber)
    {
        if (_registeredTrainNumber is not null)
        {
            await UnregisterTrain(_registeredTrainNumber.Value);
        }
        
        await _fahrenServiceClient.RegisterOnTrainAsync(new RegisterOnTrainRequest()
        {
            ClientId = _id,
            TrainNumber = trainNumber
        });
        _registeredTrainNumber = trainNumber;
    }

    public async Task UnregisterTrain()
    {
        if (_registeredTrainNumber is null)
        {
            return;
        }

        await UnregisterTrain(_registeredTrainNumber.Value);
    }
    
    private async Task UnregisterTrain(int trainNumber)
    {
        await _fahrenServiceClient.UnregisterOnTrainAsync(new UnregisterOnTrainRequest()
        {
            ClientId = _id
        });

        _registeredTrainNumber = null;
    }
    
    private void ProcessMessages(List<Message> messages)
    {
        foreach (Message message in messages)
        {
            Type? type = typeof(SharedAssemblyDefinition).Assembly.GetType(message.Type);
            object? deserializedEvent = JsonSerializer.Deserialize(message.Data.ToByteArray(), type);

            if (deserializedEvent is null)
            {
                throw new Exception("Unknown message type");
            }
            
            _sender.Send(deserializedEvent);
        }
    }

    public void Dispose()
    {
        UnregisterTrain().ConfigureAwait(false);
    }
}