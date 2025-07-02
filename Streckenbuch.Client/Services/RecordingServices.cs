using Microsoft.JSInterop;
using Streckenbuch.Shared.Services;
using Streckenbuch.Shared.Types;

namespace Streckenbuch.Client.Services;

public class RecordingServices
{
    private record Recording(GeolocationPosition Coordinate, DateTime DateTime);

    private readonly List<Recording> _recordings = new();
    private readonly RecordingService.RecordingServiceClient _recordingServiceClient;
    private readonly SettingsProvider _settingsProvider;
    private Guid? _workTripId;
    
    public RecordingServices(RecordingService.RecordingServiceClient recordingServiceClient, SettingsProvider settingsProvider)
    {
        _recordingServiceClient = recordingServiceClient;
        _settingsProvider = settingsProvider;
    }

    public void AddRecording(GeolocationPosition coordinate)
    {
        if (!_settingsProvider.IsRecordingActive)
        {
            return;
        }
        
        lock (_recordings)
        {
            _recordings.Add(new Recording(coordinate, DateTime.Now));
        }
    }

    public async Task StartWorkTrip(int trainNumber, int trainDriverNumber)
    {
        StartRecordingSessionResponse startRecordingSessionResponse = await _recordingServiceClient.StartRecordingSessionAsync(new StartRecordingSessionRequest()
        {
            TrainNumber = trainNumber, TrainDriverNumber = trainDriverNumber,
        });

        _workTripId = startRecordingSessionResponse.WorkTrip;
    }

    public void StartBackgroundTask(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(60));
            while (!cancellationToken.IsCancellationRequested)
            {
                await periodicTimer.WaitForNextTickAsync(cancellationToken);

                if (_workTripId is null)
                {
                    continue;
                }

                var list = new List<RecordPosition>();

                lock (_recordings)
                {
                    foreach (Recording recording in _recordings)
                    {
                        list.Add(new RecordPosition()
                        {
                            DateTime = recording.DateTime.Ticks,
                            Location = new LocationProto()
                            {
                                Latitude = recording.Coordinate.Coords.Latitude, Longitude = recording.Coordinate.Coords.Longitude,
                            }
                        });
                    }
                    
                    _recordings.Clear();
                }

                SendRecordedLocationsRequest sendRecordedLocationsRequest = new();
                sendRecordedLocationsRequest.Positions.AddRange(list);
                sendRecordedLocationsRequest.WorkTripId = _workTripId.Value;
                await _recordingServiceClient.SendRecordedLocationsAsync(sendRecordedLocationsRequest);
            }
        }, cancellationToken);
    }
}