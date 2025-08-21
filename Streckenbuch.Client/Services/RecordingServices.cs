using Microsoft.JSInterop;
using Streckenbuch.Components.Models;
using Streckenbuch.Components.Services;
using Streckenbuch.Shared.Services;
using Streckenbuch.Shared.Types;

namespace Streckenbuch.Client.Services;

public class RecordingServices : IRecordingServices
{
    private record Recording(GeolocationPosition Coordinate, DateTime DateTime);

    private readonly List<Recording> _recordings = new();
    private readonly RecordingService.RecordingServiceClient _recordingServiceClient;
    private readonly ISettingsProvider _settingsProvider;
    private Guid? _workTripId;

    public RecordingServices(RecordingService.RecordingServiceClient recordingServiceClient, ISettingsProvider settingsProvider)
    {
        _recordingServiceClient = recordingServiceClient;
        _settingsProvider = settingsProvider;
    }

    public void AddRecording(GeolocationPosition coordinate)
    {
        if (_settingsProvider.IsRecordingActive == RecordingOption.None)
        {
            return;
        }

        _recordings.Add(new Recording(coordinate, DateTime.Now));
    }

    public async Task StartWorkTrip(int trainNumber, int trainDriverNumber)
    {
        if (trainDriverNumber == 0)
        {
            throw new Exception("Train driver number must not be 0");
        }

        if (trainNumber == 0)
        {
            throw new Exception("Train number must not be 0");
        }

        _recordings.Clear();

        StartRecordingSessionResponse startRecordingSessionResponse = await _recordingServiceClient.StartRecordingSessionAsync(new StartRecordingSessionRequest()
        {
            TrainNumber = trainNumber,
            TrainDriverNumber = trainDriverNumber,
        });

        _workTripId = startRecordingSessionResponse.WorkTrip;
    }

    public void StartBackgroundTask(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
#if DEBUG
            using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
#else
            using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(60));
#endif
            while (!cancellationToken.IsCancellationRequested)
            {
                await periodicTimer.WaitForNextTickAsync(cancellationToken);

                if (_workTripId is null)
                {
                    continue;
                }

                if (!_recordings.Any())
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
                                Latitude = recording.Coordinate.Coords.Latitude,
                                Longitude = recording.Coordinate.Coords.Longitude,
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