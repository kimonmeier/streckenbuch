namespace Streckenbuch.Shared.Contracts;

public class StopAdded : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
    
}