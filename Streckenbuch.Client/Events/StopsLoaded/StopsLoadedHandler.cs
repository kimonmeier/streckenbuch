using MediatR;
using Streckenbuch.Client.Services;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.StopsLoaded;

public class StopsLoadedHandler : IRequestHandler<Shared.Contracts.StopsLoaded, Unit>
{
    private readonly FahrenPositionService _fahrenPositionService;
    private readonly AudioState _audioState;

    public StopsLoadedHandler(FahrenPositionService fahrenPositionService, AudioState audioState)
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