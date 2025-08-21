using MediatR;
using Streckenbuch.Components.Services;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.StopsLoaded;

public class StopsLoadedHandler : IRequestHandler<Shared.Contracts.StopsLoaded, Unit>
{
    private readonly IFahrenPositionService _fahrenPositionService;
    private readonly IAudioState _audioState;

    public StopsLoadedHandler(IFahrenPositionService fahrenPositionService, IAudioState audioState)
    {
        _fahrenPositionService = fahrenPositionService;
        _audioState = audioState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopsLoaded request, CancellationToken cancellationToken)
    {
        _fahrenPositionService.SetStops(request.BetriebspunkteId);
        
        await _audioState.SayText("Die Verbindung zum Server wurde hergestellt und die Echtzeitdaten werden nun empfangen.");
        return Unit.Value;
    }
}