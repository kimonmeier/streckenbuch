using MediatR;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.StopCanceled;

public class StopCaneledHandler : IRequestHandler<Shared.Contracts.StopCanceled, Unit>
{
    private readonly IAudioState _audioState;
    private readonly IDataState _dataState;

    public StopCaneledHandler(IAudioState audioState, IDataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopCanceled request, CancellationToken cancellationToken)
    {
        var betriebspunkt = await _dataState.FetchBetriebspunkt(request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }

        await _audioState.SayText($"Dieser Zug ist in oder ab. \"{betriebspunkt.Name}\". Ausfall");

        return Unit.Value;
    }
}