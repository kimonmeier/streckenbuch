using MediatR;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.ApproachingStop;

public class ApproachingStopEventHandler : IRequestHandler<ApproachingStopEvent, Unit>
{
    private readonly AudioState _audioState;
    private readonly DataState _dataState;
    
    public ApproachingStopEventHandler(AudioState audioState, DataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }
    
    public async Task<Unit> Handle(ApproachingStopEvent request, CancellationToken cancellationToken)
    {
        var betriebspunkt = _dataState.Betriebspunkte.SingleOrDefault(x => x.Id == request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }
        
        await _audioState.SayText($"Nächster Halt. \"{betriebspunkt.Name}\"");
        
        return Unit.Value;
    }
}