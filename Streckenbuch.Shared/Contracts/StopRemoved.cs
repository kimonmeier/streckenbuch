namespace Streckenbuch.Shared.Contracts;

public class StopRemoved : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
}