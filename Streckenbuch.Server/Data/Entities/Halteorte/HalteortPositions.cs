using Streckenbuch.Shared.Data.Entities;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Server.Data.Entities.Halteorte;

public class HalteortPositions : IEntity
{
    public Guid Id { get; set; }
    
    public Guid HalteortHeadId { get; set; }
    
    public HalteortHead HalteortHead { get; set; } = null!;
    
    public HalteortTyp Typ { get; set; }
    
    public string Description { get; set; } = null!;
}