using MediatR;
using Streckenbuch.Client.Services;

namespace Streckenbuch.Client.Events.PositionRecieved;

public class PositionRecievedEventHandler : IRequestHandler<PositionRecievedEvent>
{
    private readonly RecordingServices _recordingService;

    public PositionRecievedEventHandler(RecordingServices recordingService)
    {
        _recordingService = recordingService;
    }

    public Task Handle(PositionRecievedEvent request, CancellationToken cancellationToken)
    {
        _recordingService.AddRecording(request.Position);

        return Task.CompletedTask;
    }
}