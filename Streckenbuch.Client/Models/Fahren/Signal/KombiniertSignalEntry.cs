using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Signal;

public class KombiniertSignalEntry : ISignalEntry
{
    public EntryType Type => EntryType.Signal;

    public SignalTyp SignalTyp => SignalTyp.Kombiniert;

    public DisplaySeite SignalSeite { get; set; }

    public string? Kommentar { get; set; }

    public Coordinate Location { get; set; } = null!;
}