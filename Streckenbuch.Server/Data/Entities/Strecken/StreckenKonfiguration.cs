#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Strecken;

public class StreckenKonfiguration : IEntity
{
    public Guid Id { get; set; }

    public Guid StreckeId { get; set; }

    public Strecke Strecke { get; set; }

    public string? Name { get; set; }

    public Guid VonBetriebspunktId { get; set; }

    public Betriebspunkt VonBetriebspunkt { get; set; }

    public Guid BisBetriebspunktId { get; set; }

    public Betriebspunkt BisBetriebspunkt { get; set; }
}
