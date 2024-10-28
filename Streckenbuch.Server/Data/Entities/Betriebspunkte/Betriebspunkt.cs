#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Data.Entities;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Server.Data.Entities.Betriebspunkte;

public class Betriebspunkt : IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Coordinate Location { get; set; }

    public string? Kommentar { get; set; }

    public BetriebspunktTyp Typ { get; set; }
}
