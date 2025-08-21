using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Betriebspunkt;

public class BahnhofEntry : IBetriebspunktEntry
{

    public Guid Id { get; set; }
    
    public EntryType Type => EntryType.Betriebspunkt;

    public Coordinate Location { get; set; }

    public BetriebspunktTyp BetriebspunktTyp => BetriebspunktTyp.Bahnhof;

    public string Name { get; set; }
    
    public bool Stop { get; set; }
    
    public bool StopSpecial { get; set; }
}