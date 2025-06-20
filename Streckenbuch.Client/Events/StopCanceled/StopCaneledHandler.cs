using MediatR;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.StopCanceled;

public class StopCaneledHandler : IRequestHandler<Shared.Contracts.StopCanceled, Unit>
{
    private readonly AudioState _audioState;
    private readonly DataState _dataState;

    public StopCaneledHandler(AudioState audioState, DataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopCanceled request, CancellationToken cancellationToken)
    {
        var betriebspunkt = _dataState.Betriebspunkte.SingleOrDefault(x => x.Id == request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }

        await _audioState.SayText($"Dieser Zug ist in oder ab. \"{betriebspunkt.Name}\". Ausfall");

        return Unit.Value;
    }
}