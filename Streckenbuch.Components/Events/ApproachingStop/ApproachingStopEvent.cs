using MediatR;

namespace Streckenbuch.Components.Events.ApproachingStop;

public class ApproachingStopEvent : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
}