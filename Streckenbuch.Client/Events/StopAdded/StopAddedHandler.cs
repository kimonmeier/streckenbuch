using MediatR;
using Streckenbuch.Client.Services;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.StopAdded;

public class StopAddedHandler : IRequestHandler<Shared.Contracts.StopAdded, Unit>
{
    private readonly AudioState _audioState;
    private readonly DataState _dataState;
    private readonly FahrenPositionService _fahrenPositionService;

    public StopAddedHandler(AudioState audioState, DataState dataState, FahrenPositionService fahrenPositionService)
    {
        _audioState = audioState;
        _dataState = dataState;
        _fahrenPositionService = fahrenPositionService;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopAdded request, CancellationToken cancellationToken)
    {
        var betriebspunkt = await _dataState.FetchBetriebspunkt(request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }

        _fahrenPositionService.AddSpecialStop(betriebspunkt.Id);
        await _audioState.SayText($"Ausserplanmässiger Halt in. \"{betriebspunkt.Name}\". Ich wiederhole, Ausserplanmässiger Halt in. \"{betriebspunkt.Name}\"");
        return Unit.Value;
    }
}