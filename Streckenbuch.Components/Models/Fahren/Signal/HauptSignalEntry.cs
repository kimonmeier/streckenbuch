using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Signal;

public class HauptSignalEntry : ISignalEntry
{
    public EntryType Type => EntryType.Signal;

    public SignalTyp SignalTyp => SignalTyp.Hauptsignal;

    public DisplaySeite SignalSeite { get; set; }

    public string? Kommentar { get; set; }

    public Coordinate Location { get; set; } = null!;
}