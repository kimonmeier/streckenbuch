using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Betriebspunkt;

public class HaltestelleEntry : IBetriebspunktEntry
{

    public Guid Id { get; set; }
    
    public EntryType Type => EntryType.Betriebspunkt;

    public BetriebspunktTyp BetriebspunktTyp => BetriebspunktTyp.Haltestelle;

    public Coordinate Location { get; set; }

    public string Name { get; set; }

    public string? Kommentar { get; set; }
    
    public bool Stop { get; set; }
    
    public bool StopSpecial { get; set; }
}