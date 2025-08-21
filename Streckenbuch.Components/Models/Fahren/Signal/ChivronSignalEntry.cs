using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Signal;

public class ChivronSignalEntry : ISignalEntry
{
    public EntryType Type => EntryType.Signal;

    public SignalTyp SignalTyp => SignalTyp.Streckengeschwindigkeit;

    public Coordinate Location { get; set; } = null!;
}