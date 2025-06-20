using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Betriebspunkt;

public class DienstbahnhofEntry : IBetriebspunktEntry
{

    public Guid Id { get; set; }
    
    public EntryType Type => EntryType.Betriebspunkt;

    public BetriebspunktTyp BetriebspunktTyp => BetriebspunktTyp.Dienstbahnhof;

    public Coordinate Location { get; set; }

    public string Name { get; set; }
}