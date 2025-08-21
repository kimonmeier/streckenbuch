using MediatR;
using Streckenbuch.Components.Services;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.StopAdded;

public class StopAddedHandler : IRequestHandler<Shared.Contracts.StopAdded, Unit>
{
    private readonly IAudioState _audioState;
    private readonly IDataState _dataState;
    private readonly IFahrenPositionService _fahrenPositionService;

    public StopAddedHandler(IAudioState audioState, IDataState dataState, IFahrenPositionService fahrenPositionService)
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