using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Halteorte;

public class HalteortHead : IEntity
{
    public Guid Id { get; set; }
    
    public Guid BetriebspunktId { get; set; }

    public Betriebspunkt Betriebspunkt { get; set; } = null!;
    
    public DateOnly GueltigVon { get; set; }
    
    public DateOnly? GueltigBis { get; set; }
}