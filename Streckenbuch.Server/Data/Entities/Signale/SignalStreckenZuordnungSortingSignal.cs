using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Signale;

public class SignalStreckenZuordnungSortingSignal : IEntity
{
    public Guid Id { get; set; }
    
    public Guid SignalStreckenZuordnungId { get; set; }
    
    public SignalStreckenZuordnung SignalStreckenZuordnung { get; set; }
    
    public Guid SignalStreckenZuordnungSortingBetriebspunktId { get; set; }
    
    public SignalStreckenZuordnungSortingBetriebspunkt SignalStreckenZuordnungSortingBetriebspunkt { get; set; }
    
    public short SortingOrder { get; set; }
}