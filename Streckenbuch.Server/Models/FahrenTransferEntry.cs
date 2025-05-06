using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Server.Models;

public class FahrenTransferEntry
{
    public Betriebspunkt? Betriebspunkt { get; set; }
    public SignalStreckenZuordnung? SignalZuordnung { get; set; }
    
    public DisplaySeite? DisplaySeite { get; set; }
}
