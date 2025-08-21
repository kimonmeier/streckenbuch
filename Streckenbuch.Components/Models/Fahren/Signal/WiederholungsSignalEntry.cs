using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Signal;

public class WiederholungsSignalEntry : ISignalEntry
{
    public EntryType Type => EntryType.Signal;

    public SignalTyp SignalTyp => SignalTyp.Wiederholung;

    public Coordinate Location { get; set; } = null!;
}