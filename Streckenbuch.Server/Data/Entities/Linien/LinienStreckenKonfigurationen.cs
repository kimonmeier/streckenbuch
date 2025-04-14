using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Linien;

public class LinienStreckenKonfigurationen : IEntity
{
    public Guid Id { get; set; }

    public int Order { get; set; }

    public Guid LinieId { get; set; }

    public Linie Linie { get; set; }

    public Guid StreckenKonfigurationId { get; set; }

    public StreckenKonfiguration StreckenKonfiguration { get; set; }

    public Guid VonBetriebspunktId { get; set; }

    public Betriebspunkt VonBetriebspunkt { get; set; }

    public Guid BisBetriebspunktId { get; set; }

    public Betriebspunkt BisBetriebspunkt { get; set; }
}
