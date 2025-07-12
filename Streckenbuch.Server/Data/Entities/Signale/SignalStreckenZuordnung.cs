#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Signale;

public class SignalStreckenZuordnung : IEntity
{
    public Guid Id { get; set; }

    public Guid StreckeId { get; set; }

    public BetriebspunktStreckenZuordnung Strecke { get; set; }

    public Guid SignalId { get; set; }

    public Signal Signal { get; set; }

    public bool NonStandard { get; set; }

    public string? NonStandardKommentar { get; set; }
    
    public int? OverrideIndex { get; set; }
}
