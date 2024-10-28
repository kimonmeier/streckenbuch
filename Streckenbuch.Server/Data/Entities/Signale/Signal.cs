#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using NetTopologySuite.Geometries;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Entities;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Server.Data.Entities.Signale;

public class Signal : IEntity
{
    public Guid Id { get; set; }

    public SignalTyp Typ { get; set; }

    public Guid BetriebspunktId { get; set; }

    public Betriebspunkt Betriebspunkt { get; set; }

    public string? Name { get; set; }

    public SignalSeite Seite { get; set; }

    public Coordinate Location { get; set; }
}
