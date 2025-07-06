using blazejewicz.Blazor.BeforeUnload;
using Grpc.Core;
using MediatR;
using Streckenbuch.Client.Services;
using Streckenbuch.Shared;
using Streckenbuch.Shared.Services;
using System.Text.Json;

namespace Streckenbuch.Client.States;

public class ContinuousConnectionState
{
    private readonly FahrenService.FahrenServiceClient _fahrenServiceClient;
    private readonly SettingsProvider _settingsProvider;
    private readonly RecordingServices _recordingServices;
    private readonly Guid _id;
    private readonly ISender _sender;
    private int? _registeredTrainNumber;

    public ContinuousConnectionState(FahrenService.FahrenServiceClient fahrenServiceClient, ISender sender, RecordingServices recordingServices, SettingsProvider settingsProvider)
    {
        _fahrenServiceClient = fahrenServiceClient;
        _sender = sender;
        _recordingServices = recordingServices;
        _settingsProvider = settingsProvider;
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
        
        await _recordingServices.StartWorkTrip(trainNumber, _settingsProvider.TrainDriverNumber);
        
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

    public int? GetRegisteredTrainNumber()
    {
        return _registeredTrainNumber;
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
            try
            {
                Type? type = typeof(SharedAssemblyDefinition).Assembly.GetType(message.Type);
                object? deserializedEvent = JsonSerializer.Deserialize(message.Data, type);

                if (deserializedEvent is null)
                {
                    throw new Exception("Unknown message type");
                }

                _sender.Send(deserializedEvent);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}