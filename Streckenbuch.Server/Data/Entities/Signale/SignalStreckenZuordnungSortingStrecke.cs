using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Signale;

public class SignalStreckenZuordnungSortingStrecke : IEntity
{
    public Guid Id { get; set; }
    
    public DateOnly GueltigVon { get; set; }
    
    public DateOnly? GueltigBis { get; set; }

    public Guid StreckenKonfigurationId { get; set; }
    
    public StreckenKonfiguration StreckenKonfiguration { get; set; }
    
    public List<SignalStreckenZuordnungSortingBetriebspunkt> Betriebspunkte { get; set; }
}