using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Betriebspunkt;

public interface IBetriebspunktEntry : IBaseEntry
{
    public BetriebspunktTyp BetriebspunktTyp { get; }
}