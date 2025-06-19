using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Linien;

public class LinieTrain : IEntity
{
    public Guid Id { get; set; }
    
    public Guid LinieId { get; set; }
    
    public Linie Linie { get; set; }
    
    public int TrainNumber { get; set; }
}
