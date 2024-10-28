#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Betriebspunkte;

public class BetriebspunktStreckenZuordnung : IEntity
{
    public Guid Id { get; set; }

    public Guid StreckenKonfigurationId { get; set; }

    public StreckenKonfiguration StreckenKonfiguration { get; set; }

    public Guid BetriebspunktId { get; set; }

    public Betriebspunkt Betriebspunkt { get; set; }

    public int SortNummer { get; set; }
}
