using MediatR;
using Streckenbuch.Components.States;

namespace Streckenbuch.Components.Events.TrainSelect;

public class TrainSelectCommandHandler : IRequestHandler<TrainSelectCommand, Unit>
{
    private readonly IContinuousConnectionState _continuousConnectionState;

    public TrainSelectCommandHandler(IContinuousConnectionState continuousConnectionState)
    {
        _continuousConnectionState = continuousConnectionState;
    }

    public async Task<Unit> Handle(TrainSelectCommand request, CancellationToken cancellationToken)
    {
        await _continuousConnectionState.RegisterTrain(request.TrainNumber);
        return Unit.Value;
    }
}