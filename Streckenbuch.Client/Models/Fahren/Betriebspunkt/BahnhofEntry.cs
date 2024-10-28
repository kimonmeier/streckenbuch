using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Betriebspunkt;

public class BahnhofEntry : IBetriebspunktEntry
{
    public EntryType Type => EntryType.Betriebspunkt;

    public Coordinate Location { get; set; }

    public BetriebspunktTyp BetriebspunktTyp => BetriebspunktTyp.Bahnhof;

    public string Name { get; set; }
}