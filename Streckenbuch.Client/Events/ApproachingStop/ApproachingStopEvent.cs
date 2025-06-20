using MediatR;

namespace Streckenbuch.Client.Events.ApproachingStop;

public class ApproachingStopEvent : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
}