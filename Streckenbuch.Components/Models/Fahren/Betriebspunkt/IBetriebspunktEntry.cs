using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Betriebspunkt;

public interface IBetriebspunktEntry : IBaseEntry
{
    public Guid Id { get; set; }
    
    public BetriebspunktTyp BetriebspunktTyp { get; }
}