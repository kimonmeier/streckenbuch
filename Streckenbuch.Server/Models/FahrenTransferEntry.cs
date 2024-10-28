using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Signale;

namespace Streckenbuch.Server.Models;

public class FahrenTransferEntry
{
    public Betriebspunkt? Betriebspunkt { get; set; }
    public SignalStreckenZuordnung? SignalZuordnung { get; set; }
}
