namespace Streckenbuch.Shared.Contracts;

public class StopDelayIntroduced : IRequest<Unit>
{
    public required Guid BetriebspunktId { get; set; }
    
    public required int MinutesDelayed { get; set; }
    
    public required string Reason { get; set; }
}