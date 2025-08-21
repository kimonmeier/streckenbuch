using MediatR;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.ApproachingStop;

public class ApproachingStopEventHandler : IRequestHandler<ApproachingStopEvent, Unit>
{
    private readonly IAudioState _audioState;
    private readonly IDataState _dataState;
    
    public ApproachingStopEventHandler(IAudioState audioState, IDataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }
    
    public async Task<Unit> Handle(ApproachingStopEvent request, CancellationToken cancellationToken)
    {
        var betriebspunkt = await _dataState.FetchBetriebspunkt(request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }
        
        await _audioState.SayText($"Nächster Halt. \"{betriebspunkt.Name}\"");
        
        return Unit.Value;
    }
}