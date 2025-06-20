using MediatR;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.StopAdded;

public class StopAddedHandler : IRequestHandler<Shared.Contracts.StopAdded, Unit>
{
    private readonly AudioState _audioState;
    private readonly DataState _dataState;

    public StopAddedHandler(AudioState audioState, DataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopAdded request, CancellationToken cancellationToken)
    {
        var betriebspunkt = _dataState.Betriebspunkte.SingleOrDefault(x => x.Id == request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }

        await _audioState.SayText($"Ausserplanmässiger Halt in. \"{betriebspunkt.Name}\". Ich wiederhole, Ausserplanmässiger Halt in. \"{betriebspunkt.Name}\"");
        return Unit.Value;
    }
}