using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Shift;

public class WorkDriver : IEntity
{
    public Guid Id { get; set; }
    
    public int TrainDriverNumber { get; set; }
    
    public string? ApplicationUserId { get; set; }
    
    public ApplicationUser? ApplicationUser { get; set; }
}