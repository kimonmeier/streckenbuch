using MediatR;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.StopDelayIntroduced;

public class StopDelayIntroducedHandler : IRequestHandler<Shared.Contracts.StopDelayIntroduced, Unit>
{
    private readonly AudioState _audioState;
    private readonly DataState _dataState;

    public StopDelayIntroducedHandler(AudioState audioState, DataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopDelayIntroduced request, CancellationToken cancellationToken)
    {
        var betriebspunkt = _dataState.Betriebspunkte.SingleOrDefault(x => x.Id == request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }
        
        await _audioState.SayText($"Neue Verspätung in.\"{betriebspunkt.Name}\". \"{request.MinutesDelayed}\". Minuten. Der Grund dafür. \"{request.Reason}\"");
        return Unit.Value;
    }
}