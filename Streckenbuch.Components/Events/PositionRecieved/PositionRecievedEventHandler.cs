using MediatR;
using Streckenbuch.Components.Services;

namespace Streckenbuch.Components.Events.PositionRecieved;

public class PositionRecievedEventHandler : IRequestHandler<PositionRecievedEvent>
{
    private readonly IRecordingServices _recordingService;

    public PositionRecievedEventHandler(IRecordingServices recordingService)
    {
        _recordingService = recordingService;
    }

    public Task Handle(PositionRecievedEvent request, CancellationToken cancellationToken)
    {
        _recordingService.AddRecording(request.Position);

        return Task.CompletedTask;
    }
}