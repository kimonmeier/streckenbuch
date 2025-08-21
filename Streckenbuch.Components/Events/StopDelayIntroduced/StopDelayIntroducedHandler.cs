using MediatR;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.StopDelayIntroduced;

public class StopDelayIntroducedHandler : IRequestHandler<Shared.Contracts.StopDelayIntroduced, Unit>
{
    private readonly IAudioState _audioState;
    private readonly IDataState _dataState;

    public StopDelayIntroducedHandler(IAudioState audioState, IDataState dataState)
    {
        _audioState = audioState;
        _dataState = dataState;
    }

    public async Task<Unit> Handle(Shared.Contracts.StopDelayIntroduced request, CancellationToken cancellationToken)
    {
        var betriebspunkt = await _dataState.FetchBetriebspunkt(request.BetriebspunktId);

        if (betriebspunkt is null)
        {
            return Unit.Value;
        }
        
        await _audioState.SayText($"Neue Verspätung in.\"{betriebspunkt.Name}\". \"{request.MinutesDelayed}\". Minuten. Der Grund dafür. \"{request.Reason}\"");
        return Unit.Value;
    }
}