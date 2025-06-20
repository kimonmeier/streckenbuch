namespace Streckenbuch.Shared.Contracts;

public class StopCanceled : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
    
    public required string Reason { get; set; }
}