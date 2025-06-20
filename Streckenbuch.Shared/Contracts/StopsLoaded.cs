namespace Streckenbuch.Shared.Contracts;

public class StopsLoaded : IRequest<Unit>
{
    public required List<Guid> BetriebspunkteId { get; set; }
}