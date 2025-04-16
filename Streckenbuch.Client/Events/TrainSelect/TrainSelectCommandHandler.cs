using MediatR;
using Streckenbuch.Client.States;

namespace Streckenbuch.Client.Events.TrainSelect;

public class TrainSelectCommandHandler : IRequestHandler<TrainSelectCommand, Unit>
{
    private readonly ContinuousConnectionState _continuousConnectionState;

    public TrainSelectCommandHandler(ContinuousConnectionState continuousConnectionState)
    {
        _continuousConnectionState = continuousConnectionState;
    }

    public async Task<Unit> Handle(TrainSelectCommand request, CancellationToken cancellationToken)
    {
        await _continuousConnectionState.RegisterTrain(request.TrainNumber);
        return Unit.Value;
    }
}