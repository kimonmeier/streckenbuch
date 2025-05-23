﻿using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Signale;

public class SignalStreckenZuordnungSortingBetriebspunkt : IEntity
{
    public Guid Id { get; set; }
    public Guid SignalStreckenZuordnungSortingStreckeId { get; set; }
    
    public SignalStreckenZuordnungSortingStrecke SignalStreckenZuordnungSortingStrecke { get; set; }
    
    public Guid BetriebspunktId { get; set; }
    
    public Betriebspunkt Betriebspunkt { get; set; }
    
    public List<SignalStreckenZuordnungSortingSignal> Signale { get; set; }
}